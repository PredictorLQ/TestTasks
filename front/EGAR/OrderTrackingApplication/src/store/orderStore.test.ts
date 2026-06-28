import { beforeEach, describe, expect, it, vi } from 'vitest';
import { ordersApi } from '../api/ordersApi';
import { createMockOrder } from '../test/testUtils';
import { useOrderStore } from './orderStore';

vi.mock('../api/ordersApi', () => ({
  ordersApi: {
    getAll: vi.fn(),
    getById: vi.fn(),
    create: vi.fn(),
    updateStatus: vi.fn(),
  },
}));

describe('orderStore', () => {
  beforeEach(() => {
    useOrderStore.setState({
      orders: [],
      loading: false,
      error: null,
      trackedOrderIds: [],
    });
    localStorage.clear();
    vi.clearAllMocks();
  });

  it('fetchOrders loads orders from api', async () => {
    const orders = [createMockOrder()];
    vi.mocked(ordersApi.getAll).mockResolvedValue(orders);

    await useOrderStore.getState().fetchOrders();

    expect(useOrderStore.getState().orders).toEqual(orders);
    expect(useOrderStore.getState().loading).toBe(false);
    expect(useOrderStore.getState().error).toBeNull();
  });

  it('fetchOrders sets error on failure', async () => {
    vi.mocked(ordersApi.getAll).mockRejectedValue(new Error('Network error'));

    await useOrderStore.getState().fetchOrders();

    expect(useOrderStore.getState().orders).toEqual([]);
    expect(useOrderStore.getState().error).toBe('Network error');
  });

  it('createOrder prepends new order', async () => {
    const newOrder = createMockOrder({ orderNumber: 'ORD-NEW' });
    vi.mocked(ordersApi.create).mockResolvedValue(newOrder);

    const result = await useOrderStore.getState().createOrder({
      orderNumber: 'ORD-NEW',
      description: 'New',
    });

    expect(result).toEqual(newOrder);
    expect(useOrderStore.getState().orders[0]).toEqual(newOrder);
  });

  it('updateOrderStatus updates existing order', async () => {
    const order = createMockOrder();
    const updated = createMockOrder({ status: 'Sent' });
    useOrderStore.setState({ orders: [order] });
    vi.mocked(ordersApi.updateStatus).mockResolvedValue(updated);

    await useOrderStore.getState().updateOrderStatus(order.id, 'Sent');

    expect(useOrderStore.getState().orders[0].status).toBe('Sent');
  });

  it('tracks and untracks orders', () => {
    const id = createMockOrder().id;

    useOrderStore.getState().addTrackedOrder(id);
    expect(useOrderStore.getState().isTracked(id)).toBe(true);

    useOrderStore.getState().removeTrackedOrder(id);
    expect(useOrderStore.getState().isTracked(id)).toBe(false);
  });

  it('does not duplicate tracked order ids', () => {
    const id = createMockOrder().id;

    useOrderStore.getState().addTrackedOrder(id);
    useOrderStore.getState().addTrackedOrder(id);

    expect(useOrderStore.getState().trackedOrderIds).toEqual([id]);
  });
});
