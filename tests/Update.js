import { check } from 'k6';
import http from 'k6/http';

const BASE_URL = 'http://localhost:5239/products';

const params = {
    headers: {
        'Content-Type': 'application/json',
    }
};

export default () => {
    let productId = Math.floor(Math.random() * 100) + 1;

    const payload = JSON.stringify({
        "name": "Product 122",
        "quantity": Math.floor(Math.random() * 50) + 1
    });

    let res = http.put(`${BASE_URL}/${productId}`, payload, params);

    check(res, {
        'is status 204': (r) => r.status === 204,
    });
}