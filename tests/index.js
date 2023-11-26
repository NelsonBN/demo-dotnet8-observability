import { group, sleep } from 'k6';
import GetById from './GetById.js';
import GetList from './GetList.js';
import Create from './Create.js';
import Update from './Update.js';

export default () => {
    group('Get a product by id', () => {
        GetById();
    });

    group('Get a list of products', () => {
        GetList();
    });

    group('Create a new product', () => {
        Create();
    });

    group('update a product', () => {
        Update();
    });

    sleep(1);
}
