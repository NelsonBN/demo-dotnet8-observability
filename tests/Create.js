import { check } from 'k6';
import http from 'k6/http';

const BASE_URL = 'http://localhost:5239/products';

const params = {
    headers: {
        'Content-Type': 'application/json',
    }
};

export default () => {
    const payload = JSON.stringify({
        "name": "New Product",
        "quantity": Math.floor(Math.random() * 50) + 1
    });

    let res = http.post(BASE_URL, payload, params);

    check(res, {
        'is status 201': (r) => r.status === 201,
    });
}