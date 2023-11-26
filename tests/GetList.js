import { check } from 'k6';
import http from 'k6/http';

const BASE_URL = 'http://localhost:5239/products';

export default () => {
    let res = http.get(BASE_URL);

    check(res, {
        'is status 200': (r) => r.status === 200,
    });
}