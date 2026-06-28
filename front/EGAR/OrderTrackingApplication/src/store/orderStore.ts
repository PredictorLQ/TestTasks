import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { ordersApi } from '../api/ordersApi';
import type { CreateOrderRequest, Order, OrderStatus } from '../types/order';

interface OrderState {
  orders: Order[];
  loading: boolean;
  error: string | null;
  trackedOrderIds: string[];
  fetchOrders: () => Promise<void>;
  createOrder: (data: CreateOrderRequest) => Promise<Order>;
  updateOrderStatus: (id: string, status: OrderStatus) => Promise<Order>;
  upsertOrder: (order: Order) => void;
  addTrackedOrder: (id: string) => void;
  removeTrackedOrder: (id: string) => void;
  isTracked: (id: string) => boolean;
  clearError: () => void;
}

export const useOrderStore = create<OrderState>()(
  persist(
    (set, get) => ({
      orders: [],
      loading: false,
      error: null,
      trackedOrderIds: [],

      fetchOrders: async () => {
        set({ loading: true, error: null });
        try {
          const orders = await ordersApi.getAll();
          set({ orders, loading: false });
        } catch (error) {
          set({
            loading: false,
            error: error instanceof Error ? error.message : 'Не удалось загрузить заказы',
          });
        }
      },

      createOrder: async (data) => {
        set({ error: null });
        const order = await ordersApi.create(data);
        set((state) => ({ orders: [order, ...state.orders] }));
        return order;
      },

      updateOrderStatus: async (id, status) => {
        set({ error: null });
        const order = await ordersApi.updateStatus(id, { status });
        get().upsertOrder(order);
        return order;
      },

      upsertOrder: (order) => {
        set((state) => {
          const exists = state.orders.some((item) => item.id === order.id);
          return {
            orders: exists
              ? state.orders.map((item) => (item.id === order.id ? order : item))
              : [order, ...state.orders],
          };
        });
      },

      addTrackedOrder: (id) => {
        set((state) => ({
          trackedOrderIds: state.trackedOrderIds.includes(id)
            ? state.trackedOrderIds
            : [...state.trackedOrderIds, id],
        }));
      },

      removeTrackedOrder: (id) => {
        set((state) => ({
          trackedOrderIds: state.trackedOrderIds.filter((item) => item !== id),
        }));
      },

      isTracked: (id) => get().trackedOrderIds.includes(id),

      clearError: () => set({ error: null }),
    }),
    {
      name: 'order-tracking-storage',
      partialize: (state) => ({ trackedOrderIds: state.trackedOrderIds }),
    },
  ),
);
