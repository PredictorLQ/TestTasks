import { useCallback, useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { Alert, Button, Col, Form, Row, Spinner } from 'react-bootstrap';
import { ordersApi, ORDER_STATUSES } from '../api/ordersApi';
import { StatusBadge } from '../components/StatusBadge';
import { useOrderStatusStream } from '../hooks/useOrderStatusStream';
import { useOrderStore } from '../store/orderStore';
import type { Order, OrderStatus, OrderStatusNotification } from '../types/order';
import { formatDateTime, getStatusLabel } from '../utils/statusLabels';

export function OrderDetailPage() {
  const { id } = useParams<{ id: string }>();
  const {
    updateOrderStatus,
    upsertOrder,
    addTrackedOrder,
    removeTrackedOrder,
    isTracked,
  } = useOrderStore();

  const [order, setOrder] = useState<Order | null>(null);
  const [loading, setLoading] = useState(true);
  const [updating, setUpdating] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [liveConnected, setLiveConnected] = useState(false);
  const [lastNotification, setLastNotification] = useState<OrderStatusNotification | null>(null);
  const [selectedStatus, setSelectedStatus] = useState<OrderStatus>('Created');

  const loadOrder = useCallback(async () => {
    if (!id) {
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const data = await ordersApi.getById(id);
      setOrder(data);
      setSelectedStatus(data.status);
      upsertOrder(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Заказ не найден');
    } finally {
      setLoading(false);
    }
  }, [id, upsertOrder]);

  useEffect(() => {
    void loadOrder();
  }, [loadOrder]);

  const handleNotification = useCallback(
    (notification: OrderStatusNotification) => {
      if (notification.orderId !== id) {
        return;
      }

      setLiveConnected(true);
      setLastNotification(notification);
      setOrder((current) =>
        current
          ? { ...current, status: notification.newStatus, updatedAt: notification.occurredAt }
          : current,
      );
      upsertOrder({
        id: notification.orderId,
        orderNumber: notification.orderNumber,
        description: order?.description ?? '',
        status: notification.newStatus,
        createdAt: order?.createdAt ?? notification.occurredAt,
        updatedAt: notification.occurredAt,
      });
    },
    [id, order, upsertOrder],
  );

  useOrderStatusStream(handleNotification, id, Boolean(id));

  useEffect(() => {
    setLiveConnected(Boolean(id));
  }, [id]);

  const handleStatusUpdate = async () => {
    if (!id || !order) {
      return;
    }

    setUpdating(true);
    setError(null);

    try {
      const updated = await updateOrderStatus(id, selectedStatus);
      setOrder(updated);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Не удалось обновить статус');
    } finally {
      setUpdating(false);
    }
  };

  if (loading) {
    return (
      <div className="text-center py-5">
        <Spinner animation="border" />
      </div>
    );
  }

  if (!order) {
    return (
      <Alert variant="warning">
        Заказ не найден. <Link to="/">Вернуться к списку</Link>
      </Alert>
    );
  }

  return (
    <>
      <div className="d-flex justify-content-between align-items-start mb-4">
        <div>
          <Link to="/" className="text-decoration-none small">
            ← К списку заказов
          </Link>
          <h1 className="h3 mt-2 mb-1">Заказ {order.orderNumber}</h1>
          <p className="text-muted mb-0">{order.description}</p>
        </div>
        <div className="text-end">
          <StatusBadge status={order.status} />
          <div className="mt-2">
            <span className={`badge ${liveConnected ? 'text-bg-success' : 'text-bg-secondary'}`}>
              SSE {liveConnected ? 'подключено' : 'ожидание'}
            </span>
          </div>
        </div>
      </div>

      {error && (
        <Alert variant="danger" onClose={() => setError(null)} dismissible>
          {error}
        </Alert>
      )}

      <Row className="g-4">
        <Col lg={7}>
          <div className="card shadow-sm mb-4">
            <div className="card-body">
              <h5 className="card-title">Информация о заказе</h5>
              <dl className="row mb-0">
                <dt className="col-sm-4">ID</dt>
                <dd className="col-sm-8"><code>{order.id}</code></dd>
                <dt className="col-sm-4">Статус</dt>
                <dd className="col-sm-8"><StatusBadge status={order.status} /></dd>
                <dt className="col-sm-4">Создан</dt>
                <dd className="col-sm-8">{formatDateTime(order.createdAt)}</dd>
                <dt className="col-sm-4">Обновлён</dt>
                <dd className="col-sm-8">{formatDateTime(order.updatedAt)}</dd>
              </dl>
            </div>
          </div>

          <div className="card shadow-sm">
            <div className="card-body">
              <h5 className="card-title mb-3">Изменить статус</h5>
              <Row className="g-3 align-items-end">
                <Col md={8}>
                  <Form.Group>
                    <Form.Label>Новый статус</Form.Label>
                    <Form.Select
                      value={selectedStatus}
                      onChange={(e) => setSelectedStatus(e.target.value as OrderStatus)}
                    >
                      {ORDER_STATUSES.map((status) => (
                        <option key={status} value={status}>
                          {getStatusLabel(status)}
                        </option>
                      ))}
                    </Form.Select>
                  </Form.Group>
                </Col>
                <Col md={4}>
                  <Button
                    variant="primary"
                    className="w-100"
                    disabled={updating || selectedStatus === order.status}
                    onClick={() => void handleStatusUpdate()}
                  >
                    {updating ? 'Сохранение…' : 'Обновить'}
                  </Button>
                </Col>
              </Row>
            </div>
          </div>
        </Col>

        <Col lg={5}>
          <div className="card shadow-sm mb-4">
            <div className="card-body">
              <h5 className="card-title">Отслеживание</h5>
              <p className="text-muted small">
                Статус обновляется автоматически через Server-Sent Events (SSE).
              </p>
              <Button
                variant={isTracked(order.id) ? 'outline-danger' : 'outline-primary'}
                onClick={() =>
                  isTracked(order.id)
                    ? removeTrackedOrder(order.id)
                    : addTrackedOrder(order.id)
                }
              >
                {isTracked(order.id) ? 'Убрать из отслеживаемых' : 'Добавить в отслеживаемые'}
              </Button>
            </div>
          </div>

          <div className="card shadow-sm">
            <div className="card-body">
              <h5 className="card-title">Последнее событие</h5>
              {lastNotification ? (
                <div className="small">
                  <div>
                    {getStatusLabel(lastNotification.previousStatus)} →{' '}
                    {getStatusLabel(lastNotification.newStatus)}
                  </div>
                  <div className="text-muted mt-1">
                    {formatDateTime(lastNotification.occurredAt)}
                  </div>
                </div>
              ) : (
                <p className="text-muted small mb-0">Событий изменения статуса пока не было.</p>
              )}
            </div>
          </div>
        </Col>
      </Row>
    </>
  );
}
