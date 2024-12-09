import storeProductInformation from './store/product-information.js';

const productBoard = createApp({
    data() {
        return {
            products: []
        }
    },
    methods: {
        async getProducts() {
            try {
                const url = "/api/ProductApi/GetProducts";
                const response = await axios.get(url);
                const products = response.data;
                const productType = ["日本", "韓國"];
                const types = [...new Set(products.map(item => item.type))];
                types.forEach(type => {
                    let product = products.filter(product => product.type === type);
                    let productsCount = product.length;
                    const pageToShow = 6;
                    const startIndex = 0;
                    const endIndex = (productsCount < pageToShow) ? productsCount : pageToShow;
                    let pushContent = reactive({
                        type: type,
                        productTitle: `${productType[type - 1]}商品`,
                        items: product,
                        currentPage: 1,
                        pageCount: Math.ceil(productsCount / pageToShow),
                        startIndex: startIndex,
                        endIndex: endIndex,
                        pageToShow: pageToShow,
                        showProducts: product.slice(startIndex, endIndex)
                    })
                    this.products.push(pushContent);
                });
            }
            catch (error) {
                console.log(error);
            }
        },
        getProductInformation(productID) {
            storeProductInformation.dispatch('getProductInformation', { productID });
        },
        goToPage(product, pageNumber) {
            product.currentPage = pageNumber;
            this.adjustPaginationIndex(product);
        },
        previousPage(product) {
            if (product.currentPage <= 1) return;
            product.currentPage -= 1;
            this.adjustPaginationIndex(product);

        },
        nextPage(product) {
            if (product.currentPage >= product.pageCount) return;
            product.currentPage += 1;
            this.adjustPaginationIndex(product);
        },
        adjustPaginationIndex(product) {
            product.startIndex = (product.currentPage - 1) * product.pageToShow;
            product.endIndex = product.currentPage * product.pageToShow;
            if (product.endIndex > product.items.length) product.endIndex = product.items.length;
            product.showProducts = product.items.slice(product.startIndex, product.endIndex);
        }
    },
    computed: {
    },
    async created() {
        await this.getProducts();
    },
    mounted() {

    }
});
productBoard.use(storeProductInformation)
productBoard.mount(".product-board");

