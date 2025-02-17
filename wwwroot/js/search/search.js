import storeProductInformation from '../vue/store/product-information.js';

const searchResult = createApp({
    data() {
        return {
            pageTitle: "",
            // 排序鈕標題
            sortButtonsTitle: "篩選",
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
            const apiURL = "/api/SearchApi/Search";
            const data = { keyword: keyword };
            this.pageTitle = keyword;
            try {
                const response = await axios.get(apiURL, { params: data });
                //console.log(response);                     
                if (response.status === 200 && response.data.success === true) {
                    this.products = response.data.searchResult;
                }
            }
            catch (error) {
                //console.log(error);
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
searchResult.mount("#search-result");