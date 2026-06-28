import { Link } from 'react-router-dom';
import { Button, Table } from 'react-bootstrap';
import type { Order } from '../types/order';
import { formatDateTime } from '../utils/statusLabels';
import { StatusBadge } from './StatusBadge';
import { useOrderStore } from '../store/orderStore';

interface OrderTableProps {
  orders: Order[];
}

export function OrderTable({ orders }: OrderTableProps) {
  const { addTrackedOrder, removeTrackedOrder, isTracked } = useOrderStore();

  if (orders.length === 0) {
    return <p className="text-muted mb-0">Заказов пока нет. Создайте первый заказ.</p>;
  }

  return (
    <div className="table-responsive">
      <Table hover className="align-middle mb-0">
        <thead>
          <tr>
            <th>Номер</th>
            <th>Описание</th>
            <th>Статус</th>
            <th>Создан</th>
            <th>Обновлён</th>
            <th className="text-end">Действия</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr key={order.id}>
              <td>
                <Link to={`/orders/${order.id}`} className="fw-semibold text-decoration-none">
                  {order.orderNumber}
                </Link>
              </td>
              <td>{order.description}</td>
              <td>
                <StatusBadge status={order.status} />
              </td>
              <td>{formatDateTime(order.createdAt)}</td>
              <td>{formatDateTime(order.updatedAt)}</td>
              <td className="text-end">
                <Link
                  to={`/orders/${order.id}`}
                  className="btn btn-outline-primary btn-sm me-2"
                >
                  Детали
                </Link>
                <Button
                  variant={isTracked(order.id) ? 'outline-danger' : 'outline-secondary'}
                  size="sm"
                  onClick={() =>
                    isTracked(order.id)
                      ? removeTrackedOrder(order.id)
                      : addTrackedOrder(order.id)
                  }
                >
                  {isTracked(order.id) ? 'Убрать' : 'Отслеживать'}
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
}
