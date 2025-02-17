import store from '../vue/store/cart.js';
import { numberFormat } from '../functions/numberformat.js';

const order = createApp({
    data() {
        return {
            // 進度條
            steps: [],
            currentProgress: [],
            // 購物車
            cartTitle: "購物車",
            tableRow: {
                colClass: [],
                colName: []
            },
            cart: null,
            cartItems: null,
            // 送貨資訊
            deliveryTitle: "送貨資訊",
            orderDetails: null,
            // 訂單資訊
            billTitle: "訂單資訊",
            bill: null,
            checkText: "已確認訂購品項、金額與取貨人的身份證件姓名正確無誤",
            backBtn: "返回購物車",
            sendBtn: "送出訂單"
        }
    },
    methods: {
        async getProgress() {
            try {
                await this.$store.dispatch('getProgressAPI');
                this.steps = this.$store.getters.steps;
                this.currentProgress = this.$store.getters.currentProgress;
            }
            catch (error) {
            }
        },
        async getCart() {
            try {
                await this.$store.dispatch('getCart');
                this.cart = store.getters.cart;
                this.cartItems = this.cart.cartItems;
                this.tableRow = store.getters.tableRow;
                this.bill = {
                    數量: store.getters.quantity + "件",
                    小計: this.numberFormat(store.getters.subtotal) + " 元",
                    運費: this.numberFormat(store.getters.deliveryFee) + " 元",
                    總計: this.numberFormat(store.getters.total) + " 元"
                }
            }
            catch (error) {
            }
        },
        async getOrder() {
            const apiURL = "/api/OrderApi/GetOrder"
            try {
                const response = await axios.get(apiURL);
                if (response.status === 200 & response.data.success == true) {
                    const order = response.data.order;
                    //console.log(order);
                    //console.log(orderDetails);
                    this.orderDetails = {
                        收件人姓名: order.contactName,
                        收件人電話: order.contactPhone,
                        配送地址: order.deliveryAddress,
                        備註: order.memo,
                    }
                }       
            }
            catch (error) {
            }
        },
        numberFormat(number) {
            return numberFormat(number);
        },
        async main() {
            await this.getCart();
            if (this.cart) {
                this.getProgress();
                this.getOrder();
            }
        }
    },
    computed: {
    },
    created() {
        this.main();
    },
    mounted() {
    }
});
order.use(store);
order.mount('#order');
