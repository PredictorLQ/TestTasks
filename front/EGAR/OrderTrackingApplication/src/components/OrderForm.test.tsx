import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { ordersApi } from '../api/ordersApi';
import { createMockOrder } from '../test/testUtils';
import { useOrderStore } from '../store/orderStore';
import { OrderForm } from './OrderForm';

vi.mock('../api/ordersApi', () => ({
  ordersApi: {
    getAll: vi.fn(),
    getById: vi.fn(),
    create: vi.fn(),
    updateStatus: vi.fn(),
  },
  ORDER_STATUSES: ['Created', 'Sent', 'Delivered', 'Cancelled'],
  createOrderStatusEventSource: vi.fn(),
}));

describe('OrderForm', () => {
  beforeEach(() => {
    useOrderStore.setState({ orders: [], loading: false, error: null, trackedOrderIds: [] });
    vi.clearAllMocks();
  });

  it('submits new order', async () => {
    const user = userEvent.setup();
    const created = createMockOrder({ orderNumber: 'ORD-FORM', description: 'From form' });
    vi.mocked(ordersApi.create).mockResolvedValue(created);

    render(<OrderForm />);

    await user.type(screen.getByLabelText('Номер заказа'), 'ORD-FORM');
    await user.type(screen.getByLabelText('Описание'), 'From form');
    await user.click(screen.getByRole('button', { name: 'Создать' }));

    await waitFor(() => {
      expect(ordersApi.create).toHaveBeenCalledWith({
        orderNumber: 'ORD-FORM',
        description: 'From form',
      });
    });

    expect(useOrderStore.getState().orders[0]).toEqual(created);
  });

  it('shows error when create fails', async () => {
    const user = userEvent.setup();
    vi.mocked(ordersApi.create).mockRejectedValue(new Error('Duplicate order'));

    render(<OrderForm />);

    await user.type(screen.getByLabelText('Номер заказа'), 'ORD-DUP');
    await user.type(screen.getByLabelText('Описание'), 'Duplicate');
    await user.click(screen.getByRole('button', { name: 'Создать' }));

    expect(await screen.findByText('Duplicate order')).toBeInTheDocument();
  });
});
