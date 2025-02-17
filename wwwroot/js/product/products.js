import storeProductInformation from '../vue/store/product-information.js';

const products = createApp({
    data() {
        return {
            sortButtonTitle: "篩選",
            // 排序鈕
            sortButtons: ["綜合排名", "最新", "最熱銷", "價格高到低", "價格低到高"],
            sortType: "綜合排名",
            // 商品列
            products: null
        }
    },
    methods: {
        setPageTitle() {
            const searchParam = window.location.search;
            const queryString = new URLSearchParams(searchParam);
            const productType = queryString.get('type');
            switch (productType) {
                case "0":
                    this.pageTitle = "所有商品";
                    break;
                case "1":
                    this.pageTitle = "日本商品";
                    break;
                case "2":
                    this.pageTitle = "韓國商品";
                    break;
                default:
                    this.pageTitle = "商品資訊";
            }
            document.title = `商品列 - ${this.pageTitle} | Jay's Shop`;
            //console.log(queryString.get('type'));
        },
        sortProducts(sortType) {
            this.sortType = sortType;
            switch (sortType) {

            }
        },
        async getProducts() {
            const apiURL = "/api/ProductApi/GetProducts";
            const queryString = window.location.search;
            const urlParams = new URLSearchParams(queryString);
            const type = urlParams.get('type');
            //console.log(queryString);
            const apiParams = {
                type: type
            };
            try {
                const response = await axios.get(apiURL, { params: apiParams });
                if (response.status === 200 && response.data.success === true) {
                    this.products = response.data.products;
                }                                                                                  
                //console.log(response);
            }
            catch (error) {
            }
        },
        goToProductPage(productID) {
            if (productID != undefined) {
                storeProductInformation.dispatch('goToProductPageAPI', { productID });
            }
        }
    },
    async created() {
        await this.getProducts();
        this.setPageTitle();
    }
});
products.mount('#products');