import storeProductInformation from './store/product-information.js';

const search = createApp({
    data() {
        return { 
            // 頁面標題
            pageTitle:null,
            // 排序鈕
            sortButtons: ["綜合排名", "最新", "最熱銷", "價格高到低", "價格低到高"],
            sortType: "綜合排名",
            // 商品列
            products: null
        }
    },
    methods: {
        async search() {
            const searchParam = window.location.search;
            const queryString = new URLSearchParams(searchParam);
            const keyword = queryString.get('keyword');
            const url = "/api/SearchApi/Search";
            const data = { keyword: keyword };
            this.pageTitle = keyword;
            try {
                const response = await axios.get(url, { params: data });
                const products = response.data;
                if (products.length > 0) {
                    this.products = products;
                    
                    //console.log(products);    
                }
            }
            catch (error) {
                console.log(error);
            }
        },
        sortProducts(sortType) {
            this.sortType = sortType;
            switch (sortType) {

            }
        },
        getProductInformation(productID) {
            storeProductInformation.dispatch('getProductInformation', { productID });
        },
        async main() {
            await this.search();
        }
    },
    async created() {
        await this.main();
        document.title = `搜尋 - ${this.pageTitle} | Jay's Shop`;
    }
});
search.mount("#search");