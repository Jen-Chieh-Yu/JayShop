import store from './store/cart.js';
import { numberFormat } from './functions/numberformat.js';

const shoppingCart = createApp({
    data() {
        return {
            isReady: false,
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
            cartItemsCount: 0,
            // 送貨資訊
            deliveryTitle: "送貨資訊",
            deliveryDetails: [
                { label: "收件人姓名", labelFor: "contactName", placeholder: "請輸入名字", formValue: "" },
                { label: "收件人電話", labelFor: "contactPhone", placeholder: "請輸入電話號碼", formValue: "" },
                { label: "送貨方式", labelFor: "deliveryMethod", deliveryMethod: "宅配『貨到付款』", formValue: 0 },
                //{ label: "縣市區域", labelFor: "citydistrict", cities: [], districts: [] },
                { label: "縣市", labelFor: "cityCode", cities: [], formValue: 0 },
                { label: "區域", labelFor: "districtCode", districts: [], formValue: 0 },
                { label: "街道地址", labelFor: "streetAddress", placeholder: "請輸入街道地址", formValue: "" },
                { label: "備註", labelFor: "memo", placeholder: "想說些什麼嗎？", formValue: "" }
            ],
            currentCityCode: 0,
            currentDistrictCode: 0,
            // 訂單資訊
            billTitle: "訂單資訊",
            backBtn: "繼續購物",
            nextBtn: "前往結帳"
        }
    },
    methods: {
        async fetchProgresss() {
            try {
                await store.dispatch('fetchProgress');
                this.steps = store.getters.steps;
                this.currentProgress = store.getters.currentProgress;
            }
            catch (error) {
            }
        },
        async fetchCart() {
            try {
                await store.dispatch('fetchCart');                  
                this.cart = store.getters.cart;                    
                this.cartItemsCount = this.cart.cartItems.length;             
                this.cartItemsCount = this.cart.cartItems.length;             
                this.tableRow = store.getters.tableRow;
                if (this.cartItemsCount > 0) {
                    this.isReady = true;
                }
                else {
                    this.isReady = false;
                }
            }
            catch (error) {
            }
        },
        async adjustItemQuantity(event, itemID) {
            const method = event.currentTarget.innerText;
            await store.dispatch('adjustItemQuantity', { method, itemID });  
            //console.log(store.getters.cart);
        },
        async deleteItem(itemID) {
            await store.dispatch('removeItem', { itemID });
            await this.fetchCart();
            //console.log(store.getters.cart);
        },
        async fetchCity() {
            const url = "/api/AddressApi/GetCity";
            const targetIndex = 3;
            const defaultCityCode = 0;
            try {
                const unshiftParams = {
                    cityCode: defaultCityCode,
                    cityName: "請選擇縣市"
                }
                const response = await axios.get(url);
                // Insert default city option
                response.data.unshift(unshiftParams);
                this.deliveryDetails[targetIndex].formValue = defaultCityCode;
                this.deliveryDetails[targetIndex].cities = response.data;
                this.fetchDistrict(0);
                //console.log(response.data);
            }
            catch (error) {
            }
        },
        async fetchDistrict(currentCityCode) {
            const url = "/api/AddressApi/GetDistrict";
            const targetIndex = 4;
            const defaultDistrictCode = 0;
            const apiParams = {
                params: {
                    cityCode: currentCityCode
                }
            };
            const unsfitParams = {
                districtCode: 0,
                districtCode: defaultDistrictCode,
                districtName: "請選擇區域"
            };

            if (currentCityCode === 0) {
                this.deliveryDetails[targetIndex].formValue = 0;
                this.deliveryDetails[targetIndex].districts = [unsfitParams];
                //console.log(this.deliveryDetails[targetIndex].districts);
            }
            else {
                try {
                    const response = await axios.get(url, apiParams);
                    response.data.unshift(unsfitParams);
                    this.deliveryDetails[targetIndex].formValue = defaultDistrictCode;
                    this.deliveryDetails[targetIndex].districts = response.data;
                    //console.log(this.deliveryDetails[targetIndex].districts);
                }
                catch (error) {
                }
            }
        },
        selectedOption(currentCityCode) {
            this.fetchDistrict(currentCityCode);
        },
        async createOrder() {
            const apiURL = "/api/OrderApi/CreateOrder";
            const orderParams = {};
            this.deliveryDetails.forEach((detail) => {
                orderParams[detail.labelFor] = detail.formValue;
            });                                                         
            try {
                await axios.post(apiURL, orderParams);
                const response = await axios.get("/api/OrderApi/GetOrder");
                //console.log(response.data);
            }
            catch (error) { }
            //console.log(orderParams);
        },
        async goToCheckOut() {
            const href = "/Order/Order";
            await this.createOrder();
            window.location.href = href;
        },
        numberFormat(number) {
            return numberFormat(number);
        },
        async main() {
            await this.fetchCart();
            if (this.cart) {
                this.fetchProgresss();
                await this.fetchCity();           
                //await this.fetchDistrict(defaultDistrictCode);
                //console.log(store.getters.cart);
            }
            document.getElementById('shopping-cart').style.display = 'block';
        }
    },
    computed: {
        cartItems() {
            if (store.getters.cartItems)
                return store.getters.cartItems;
        },
        ...Vuex.mapGetters(['quantity', 'subtotal', 'deliveryFee', 'total']),
        bill() {
            return {
                數量: this.quantity + " 件",
                小計: this.numberFormat(this.subtotal) + " 元",
                運費: this.numberFormat(this.deliveryFee) + " 元",
                總計: this.numberFormat(this.total) + " 元"
            }
        }
    },
    async created() {
        await this.main();
    },
    mounted() {
    }
});
shoppingCart.use(store);
shoppingCart.mount('#shopping-cart');
