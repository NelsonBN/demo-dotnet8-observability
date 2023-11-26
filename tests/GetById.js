import { check } from 'k6';
import http from 'k6/http';

const BASE_URL = 'http://localhost:5239/products';

export default () => {
    let productId = Math.floor(Math.random() * 100) + 1;

    let res = http.get(`${BASE_URL}/${productId}`);

    check(res, {
        'is status 200': (r) => r.status === 200,
    });
}