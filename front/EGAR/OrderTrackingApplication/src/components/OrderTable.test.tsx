import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { MemoryRouter } from 'react-router-dom';
import { beforeEach, describe, expect, it } from 'vitest';
import { createMockOrder } from '../test/testUtils';
import { useOrderStore } from '../store/orderStore';
import { OrderTable } from './OrderTable';

function renderTable(orders = [createMockOrder()]) {
  return render(
    <MemoryRouter>
      <OrderTable orders={orders} />
    </MemoryRouter>,
  );
}

describe('OrderTable', () => {
  beforeEach(() => {
    useOrderStore.setState({ orders: [], loading: false, error: null, trackedOrderIds: [] });
  });

  it('renders empty state', () => {
    renderTable([]);

    expect(screen.getByText(/Заказов пока нет/)).toBeInTheDocument();
  });

  it('renders order rows', () => {
    renderTable([createMockOrder({ orderNumber: 'ORD-TABLE' })]);

    expect(screen.getByText('ORD-TABLE')).toBeInTheDocument();
    expect(screen.getByText('Test order')).toBeInTheDocument();
    expect(screen.getAllByText('Создан').length).toBeGreaterThanOrEqual(1);
  });

  it('adds order to tracked list', async () => {
    const user = userEvent.setup();
    const order = createMockOrder();
    renderTable([order]);

    await user.click(screen.getByRole('button', { name: 'Отслеживать' }));

    expect(useOrderStore.getState().isTracked(order.id)).toBe(true);
    expect(screen.getByRole('button', { name: 'Убрать' })).toBeInTheDocument();
  });
});
