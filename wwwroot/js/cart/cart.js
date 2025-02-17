import store from '../vue/store/cart.js';
import { numberFormat } from '../functions/numberformat.js';

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
                {
                    field: "contactName", label: "收件人姓名", labelFor: "contactName", placeholder: "請輸入名字", formValue: ""
                },
                {
                    field: "contactPhone",
                    label: "收件人電話", labelFor: "contactPhone", placeholder: "請輸入電話號碼", formValue: ""
                },
                {
                    field: "deliveryMethod", label: "送貨方式", labelFor: "deliveryMethod", deliveryMethod: "宅配『貨到付款』", formValue: 0
                },
                {
                    field: "cityDistrict",
                    cityDistrictGroup:
                    {
                        city: {
                            label: "縣市", labelFor: "cityCode", cityList: [], formValue: 0
                        },
                        district: {
                            label: "區域", labelFor: "districtCode", districtList: [], formValue: 0
                        }
                    }
                },
                {
                    field: "streetAddress", label: "街道地址", labelFor: "streetAddress", placeholder: "請輸入街道地址", formValue: ""
                },
                {
                    field: "memo", label: "備註", labelFor: "memo", placeholder: "想說些什麼嗎？", formValue: ""
                }
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
        async getProgresss() {
            try {
                await store.dispatch('getProgressAPI');
                this.steps = store.getters.steps;
                this.currentProgress = store.getters.currentProgress;
            }
            catch (error) {
            }
        },
        async getCart() {
            try {
                await store.dispatch('getCartAPI');
                this.cart = store.getters.cart;
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
            await store.dispatch('adjustItemQuantityAPI', { method, itemID });
            //console.log(store.getters.cart);
        },
        async deleteItem(itemID) {
            await store.dispatch('deleteItemAPI', { itemID });
            await this.getCart();
            //console.log(store.getters.cart);
        },
        async initialCityList() {
            const apiURL = "/api/AddressApi/GetCity";
            const defaultParams = {
                cityCode: 0,
                cityName: "請選擇縣市"
            };
            try {
                const response = await axios.get(apiURL);
                if (response) {
                    const cityDistrictGroup = this.deliveryDetails.find(field => field.field === 'cityDistrict').cityDistrictGroup;
                    response.data.unshift(defaultParams);
                    cityDistrictGroup.city.cityList = response.data;
                    this.initialDistrictList();
                }
            }
            catch (error) { }
        },
        async initialDistrictList() {
            const cityDistrictGroups = this.deliveryDetails.find(field => field.field === 'cityDistrict').cityDistrictGroup;
            const cityCode = cityDistrictGroups.city.formValue;

            const defaultParams = {
                districtCode: 0,
                districtName: "請選擇區域"
            };
            if (cityCode <= 0) {
                cityDistrictGroups.district.formValue = 0;
                cityDistrictGroups.district.districtList = [defaultParams];
            }
            else {
                const apiURL = "/api/AddressApi/GetDistrict";
                const apiParams = {
                    params: {
                        cityCode: cityCode
                    }
                };

                try {
                    const response = await axios.get(apiURL, apiParams);
                    if (response) {
                        response.data.unshift(defaultParams);
                        cityDistrictGroups.district.districtList = response.data;
                    }
                }
                catch (error) { }
            }
        },
        async createOrder() {
            const apiURL = "/api/OrderApi/CreateOrder";
            const orderForm = {};
            this.deliveryDetails.forEach((row) => {
                if (row.field === 'cityDistrict') {
                    orderForm[row.cityDistrictGroup.city.labelFor] = row.cityDistrictGroup.city.formValue;
                    orderForm[row.cityDistrictGroup.district.labelFor] = row.cityDistrictGroup.district.formValue;
                }
                else {
                    orderForm[row.labelFor] = row.formValue;
                }
            });
            //console.log(orderForm);
            try {
                const response = await axios.post(apiURL, orderForm);
                if (response.status === 200 && response.data.success === true) {
                    return response.data;
                }
            }
            catch (error) { }
            //console.log(orderParams);
        },
        async goToCheckOut() {
            const result = await this.createOrder();
            if (result) {
                window.location.href = result.href; 
                //console.log(response);
            }

            //window.location.href = href;
        },
        numberFormat(number) {
            return numberFormat(number);
        },
        async main() {
            await this.getCart();
            if (this.cart) {
                this.getProgresss();
                await this.initialCityList();
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
