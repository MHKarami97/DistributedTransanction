import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
    vus: 10,
    duration: '30s',
};
export default function () {
    http.post("http://localhost:5002/Order/Make",
        JSON.stringify({
            instrumentName: "Ins1",
            quantity: 20,
            price: 300,
            customerId: 1
        }), { headers: { 'Content-Type': 'application/json' } });
    sleep(1);
}