import { useState } from 'react';
import { Alert, Button, Col, Form, Row } from 'react-bootstrap';
import { useOrderStore } from '../store/orderStore';

export function OrderForm() {
  const createOrder = useOrderStore((state) => state.createOrder);
  const [orderNumber, setOrderNumber] = useState('');
  const [description, setDescription] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    setSubmitting(true);
    setError(null);

    try {
      await createOrder({ orderNumber, description });
      setOrderNumber('');
      setDescription('');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Ошибка создания заказа');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="card shadow-sm mb-4">
      <div className="card-body">
        <h5 className="card-title mb-3">Создать заказ</h5>
        {error && (
          <Alert variant="danger" onClose={() => setError(null)} dismissible>
            {error}
          </Alert>
        )}
        <Form onSubmit={handleSubmit}>
          <Row className="g-3">
            <Col md={4}>
              <Form.Group controlId="orderNumber">
                <Form.Label>Номер заказа</Form.Label>
                <Form.Control
                  value={orderNumber}
                  onChange={(e) => setOrderNumber(e.target.value)}
                  placeholder="ORD-001"
                  required
                  maxLength={50}
                />
              </Form.Group>
            </Col>
            <Col md={6}>
              <Form.Group controlId="description">
                <Form.Label>Описание</Form.Label>
                <Form.Control
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                  placeholder="Описание заказа"
                  required
                  maxLength={500}
                />
              </Form.Group>
            </Col>
            <Col md={2} className="d-flex align-items-end">
              <Button type="submit" variant="primary" className="w-100" disabled={submitting}>
                {submitting ? 'Создание…' : 'Создать'}
              </Button>
            </Col>
          </Row>
        </Form>
      </div>
    </div>
  );
}
