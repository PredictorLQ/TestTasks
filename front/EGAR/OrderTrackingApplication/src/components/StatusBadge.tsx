import { Badge } from 'react-bootstrap';
import type { OrderStatus } from '../types/order';
import { getStatusLabel, getStatusVariant } from '../utils/statusLabels';

interface StatusBadgeProps {
  status: OrderStatus;
}

export function StatusBadge({ status }: StatusBadgeProps) {
  return <Badge bg={getStatusVariant(status)}>{getStatusLabel(status)}</Badge>;
}
