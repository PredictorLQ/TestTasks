import { useCallback, useEffect } from 'react';
import { Alert, Col, Row, Spinner } from 'react-bootstrap';
import { OrderForm } from '../components/OrderForm';
import { OrderTable } from '../components/OrderTable';
import { TrackedOrdersPanel } from '../components/TrackedOrdersPanel';
import { useOrderStatusStream } from '../hooks/useOrderStatusStream';
import { useOrderStore } from '../store/orderStore';
import type { OrderStatusNotification } from '../types/order';

export function OrdersPage() {
  const { orders, loading, error, fetchOrders, upsertOrder, trackedOrderIds, clearError } =
    useOrderStore();

  useEffect(() => {
    void fetchOrders();
  }, [fetchOrders]);

  const handleNotification = useCallback(
    (notification: OrderStatusNotification) => {
      if (!trackedOrderIds.includes(notification.orderId)) {
        return;
      }

      void fetchOrders();
      void upsertOrder({
        id: notification.orderId,
        orderNumber: notification.orderNumber,
        description:
          orders.find((order) => order.id === notification.orderId)?.description ?? '',
        status: notification.newStatus,
        createdAt:
          orders.find((order) => order.id === notification.orderId)?.createdAt ??
          notification.occurredAt,
        updatedAt: notification.occurredAt,
      });
    },
    [trackedOrderIds, fetchOrders, upsertOrder, orders],
  );

  useOrderStatusStream(handleNotification, undefined, trackedOrderIds.length > 0);

  return (
    <>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h1 className="h3 mb-1">Заказы</h1>
          <p className="text-muted mb-0">Список заказов и создание новых</p>
        </div>
        {trackedOrderIds.length > 0 && (
          <span className="badge text-bg-info">
            SSE: отслеживается {trackedOrderIds.length} заказ(ов)
          </span>
        )}
      </div>

      {error && (
        <Alert variant="danger" onClose={clearError} dismissible>
          {error}
        </Alert>
      )}

      <Row className="g-4">
        <Col lg={8}>
          <OrderForm />
          <div className="card shadow-sm">
            <div className="card-body">
              <div className="d-flex justify-content-between align-items-center mb-3">
                <h5 className="card-title mb-0">Список заказов</h5>
                {loading && <Spinner animation="border" size="sm" />}
              </div>
              <OrderTable orders={orders} />
            </div>
          </div>
        </Col>
        <Col lg={4}>
          <TrackedOrdersPanel />
        </Col>
      </Row>
    </>
  );
}
