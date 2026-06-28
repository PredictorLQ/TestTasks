import type {
  CreateOrderRequest,
  Order,
  OrderStatus,
  UpdateOrderStatusRequest,
} from '../types/order';

const API_BASE = import.meta.env.VITE_API_URL ?? '';

async function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || `HTTP ${response.status}`);
  }

  return response.json() as Promise<T>;
}

export const ordersApi = {
  getAll(): Promise<Order[]> {
    return fetch(`${API_BASE}/api/orders`).then((r) => handleResponse<Order[]>(r));
  },

  getById(id: string): Promise<Order> {
    return fetch(`${API_BASE}/api/orders/${id}`).then((r) => handleResponse<Order>(r));
  },

  create(data: CreateOrderRequest): Promise<Order> {
    return fetch(`${API_BASE}/api/orders`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    }).then((r) => handleResponse<Order>(r));
  },

  updateStatus(id: string, data: UpdateOrderStatusRequest): Promise<Order> {
    return fetch(`${API_BASE}/api/orders/${id}/status`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    }).then((r) => handleResponse<Order>(r));
  },
};

export function createOrderStatusEventSource(orderId?: string): EventSource {
  const url = orderId
    ? `${API_BASE}/api/ordernotifications/stream?orderId=${orderId}`
    : `${API_BASE}/api/ordernotifications/stream`;

  return new EventSource(url);
}

export const ORDER_STATUSES: OrderStatus[] = ['Created', 'Sent', 'Delivered', 'Cancelled'];
