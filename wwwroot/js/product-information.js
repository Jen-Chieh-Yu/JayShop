﻿import storeProductInformation from './store/product-information.js';
import store from './store/cart.js';
import { numberFormat } from "./functions/numberformat.js";

const productInformation = createApp({
    data() {
        return {
            // 麵包屑導引
            breadCrumb: null,
            // 產品資訊
            productID: null,
            product: null,
            information: `品味盛夏，餅乾飲料絕配！
            我們的餅乾精選優質原料，口感酥脆，滿足您的味蕾；
            而清新飲品則搭配天然水果，為您帶來瞬間的清涼感受。
            現在購買組合優惠，讓您在炎炎夏日中，品味無窮的美好滋味。
            不僅是口腹之慾的享受，更是與親朋好友共度時光的最佳選擇。
            餅乾、飲料，我們為您帶來美味與愉悅，讓每一刻都成為難忘的味覺饗宴！` ,
            count: 1,
            minCount: 1,
            // 相關商品
            relevantProducts: null,
            startIndex: 0,
            endIndex: 0,
            productsToShow: 6,
            showLoadMore: true,
            // Modal
            modalShow: false
        }
    },
    methods: {
        products(type) {
            if (type === '#') return;
            const href = `/Home/Products?type=${type}`;
            try {
                window.location.href = href;
            }
            catch (error) {
            }
        },
        async fetchBreadCrumb() {
            await storeProductInformation.dispatch('fetchBreadCrumb');
            this.breadCrumb = storeProductInformation.getters.breadCrumb;
            //console.log(this.breadCrumb);
        },
        async fetchProduct() {
            const queryString = window.location.search;
            const urlParams = new URLSearchParams(queryString);
            this.productID = urlParams.get('id');
            // console.log(this.productID);
            await storeProductInformation.dispatch('fetchProduct', { productID: this.productID });
            this.product = storeProductInformation.getters.targetProduct;
            this.relevantProducts = storeProductInformation.getters.relevantProducts;
        },
        adjustQuantity(event) {
            const methods = event.currentTarget.innerText;
            //console.log(event.currentTarget.innerText);
            if (methods === "+") {
                this.count++;
            } else if (methods === "-" && this.count > 1) {
                this.count--;
            }
            //console.log(this.count);
        },
        trackBtn() {
            const url = "";
            const data = { product_id: this.productID };
        },
        async addToCart() {
            const url = `/api/CartApi/AddToCart?product_id=${this.productID}`;           
            try {
                const response = await axios.put(url);            
                if (response) {                  
                    store.dispatch('fetchCart');     
                    //console.log("Success");
                }
            }
            catch (error) {
            }
        },
        getProductInformation(productID) {
            storeProductInformation.dispatch('getProductInformation', { productID });
        },
        loadMore() {
            this.productsToShow += this.productsToShow;
            this.showLoadMore = this.endIndex + this.productsToShow < this.relevantProducts.length;
            //console.log(this.endIndex + this.productsToShow);
        },
        async main() {              
            await this.fetchProduct();
            await this.fetchBreadCrumb();
            document.getElementById('product-information').style.display = 'block';
        }
    },
    computed: {
        subTotal() {
            return numberFormat(this.product.price * this.count);
        },
        minusDisable() {
            return (this.count <= this.minCount) ? true : false;
        },
        productList() {
            return this.relevantProducts.slice(this.startIndex, this.endIndex + this.productsToShow);
        }
    },
    created() {
        this.main();
        //console.log(this.product);
    },
    mounted() {
    }
});
productInformation.mount("#product-information");