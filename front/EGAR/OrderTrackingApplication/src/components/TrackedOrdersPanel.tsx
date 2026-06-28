import { Link } from 'react-router-dom';
import { Button, ListGroup } from 'react-bootstrap';
import { useOrderStore } from '../store/orderStore';
import { StatusBadge } from './StatusBadge';

export function TrackedOrdersPanel() {
  const { orders, trackedOrderIds, removeTrackedOrder } = useOrderStore();

  const trackedOrders = trackedOrderIds
    .map((id) => orders.find((order) => order.id === id))
    .filter((order): order is NonNullable<typeof order> => order !== undefined);

  return (
    <div className="card shadow-sm">
      <div className="card-body">
        <h5 className="card-title mb-3">Отслеживаемые заказы</h5>
        {trackedOrders.length === 0 ? (
          <p className="text-muted small mb-0">
            Добавьте заказы в отслеживание из списка или со страницы деталей.
          </p>
        ) : (
          <ListGroup variant="flush">
            {trackedOrders.map((order) => (
              <ListGroup.Item
                key={order.id}
                className="d-flex justify-content-between align-items-center px-0"
              >
                <div>
                  <Link to={`/orders/${order.id}`} className="text-decoration-none fw-semibold">
                    {order.orderNumber}
                  </Link>
                  <div className="mt-1">
                    <StatusBadge status={order.status} />
                  </div>
                </div>
                <Button
                  variant="link"
                  size="sm"
                  className="text-danger text-decoration-none"
                  onClick={() => removeTrackedOrder(order.id)}
                >
                  ×
                </Button>
              </ListGroup.Item>
            ))}
          </ListGroup>
        )}
      </div>
    </div>
  );
}
