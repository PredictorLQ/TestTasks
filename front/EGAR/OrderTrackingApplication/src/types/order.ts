export type OrderStatus = 'Created' | 'Sent' | 'Delivered' | 'Cancelled';

export interface Order {
  id: string;
  orderNumber: string;
  description: string;
  status: OrderStatus;
  createdAt: string;
  updatedAt: string;
}

export interface CreateOrderRequest {
  orderNumber: string;
  description: string;
}

export interface UpdateOrderStatusRequest {
  status: OrderStatus;
}

export interface OrderStatusNotification {
  orderId: string;
  orderNumber: string;
  previousStatus: OrderStatus;
  newStatus: OrderStatus;
  occurredAt: string;
}
