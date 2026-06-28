import type { OrderStatus } from '../types/order';

const STATUS_LABELS: Record<OrderStatus, string> = {
  Created: 'Создан',
  Sent: 'Отправлен',
  Delivered: 'Доставлен',
  Cancelled: 'Отменён',
};

const STATUS_VARIANT: Record<OrderStatus, string> = {
  Created: 'secondary',
  Sent: 'primary',
  Delivered: 'success',
  Cancelled: 'danger',
};

export function getStatusLabel(status: OrderStatus): string {
  return STATUS_LABELS[status];
}

export function getStatusVariant(status: OrderStatus): string {
  return STATUS_VARIANT[status];
}

export function formatDateTime(value: string): string {
  return new Date(value).toLocaleString('ru-RU');
}
