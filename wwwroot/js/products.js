import storeProductInformation from './store/product-information.js';

const products = createApp({
    data() {
        return {
            // 排序鈕
            sortButtons: ["綜合排名", "最新", "最熱銷", "價格高到低", "價格低到高"],
            sortType: "綜合排名",
            // 商品列
            products: null
        }
    },
    methods: {
        sortProducts(sortType) {
            this.sortType = sortType;
            switch (sortType) {

            }
        },
        async fetchProducts() {
            const url = "/api/ProductApi/Products";
            const queryString = window.location.search;
            const urlParams = new URLSearchParams(queryString);
            const type = urlParams.get('type');
            //console.log(queryString);
            const params = {
                type: type
            };
            try {
                const response = await axios.get(url, { params });
                if (response) this.products = response.data;
                //console.log(response);
            }
            catch (error) {
            }
        },
        getProductInformation(productID) {
            storeProductInformation.dispatch('getProductInformation', { productID });
        }
    },
    async created() {
        await this.fetchProducts();
    }
});
products.mount('#products');