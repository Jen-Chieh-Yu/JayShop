const storeProductInformation = createStore({
    // 響應式的資料狀態儲存, 資料狀態變化時，有用到的 component 都會即時更新。
    state: {
        // 排序鈕
        // 麵包屑
        breadCrumb: null,
        // 指定商品內容
        targetProduct: null,
        // 相關商品
        relevantProducts: null
    },
    // 改變state。
    // 只處理同步函數：不要在這進行非同步的動作（例如 setTimeout / 打 API 取遠端資料...等）。
    mutations: {
        setBreadCrumb(state) {
            const breadCrumb = [
                { type: 0, hrefName: "所有商品" },
                { type: 0, hrefName: "" },
                { type: "#", hrefName: "食物" },
            ];
            const productType = state.targetProduct.type;
            breadCrumb[1].type = productType;
            switch (productType) {
                case 1:
                    breadCrumb[1].hrefName = "日本商品";
                    break;
                case 2:
                    breadCrumb[1].hrefName = "韓國商品";
                    break;
                default:
                    break;
            }
            state.breadCrumb = breadCrumb;
            //console.log(state.breadCrumb);
        },
        setProductInformation(state, data) {
            state.targetProduct = data.targetProduct;
            state.relevantProducts = data.relevantProducts;
        }
    },
    // 操作同步或異步事件的處理但不直接修改資料（state）。
    // 是透過commit → 呼叫 mutation 改變 state。
    actions: {
        async getBreadCrumbAPI({ commit }) {
            try {
                commit('setBreadCrumb');
            }
            catch (error) {
            }
        },
        async getProductInformationAPI({ commit }, { productID }) {
            const apiURL = "/api/ProductApi/GetProductInformation";
            const data = { product_id: productID };
            let result;

            try {
                const response = await axios.get(apiURL, { params: data });
                if (response.status === 200 && response.data.success === true) {
                    const productInformation = response.data.productInformation;
                    commit('setProductInformation', productInformation);
                    result = { success: true, message: "" };
                }
                else {
                    result = { success: false, message: "" };
                }
            }
            catch (error) {
                console.log("API Error", error);
                result = { success: false, message: "" };
            }
            //return result;
        },
        async addToCartAPI({ commit }, { productID, productQuantity }) {
            const apiURL = "/api/CartApi/AddToCart";
            const apiParams = { product_id: productID, product_quantity: productQuantity };
            let result;

            try {
                const response = await axios.put(apiURL, null, { params: apiParams });
                if (response.status === 200 && response.data.success === true) {
                    result = { success: true, message: "" };
                }
                else {
                    result = { success: false, message: "" };
                }
            }
            catch (error) {
                result = { success: false, message: "" };
                console.log("API Error : ", error);
            }
            return result;
        },
        goToProductPageAPI({ commit }, { productID }) {
            const href = `/Product/ProductInformation?id=${productID}`;
            try {
                window.location.href = href;
            }
            catch (error) {
            }
        }
    },
    // 加工資料呈現。
    // 同 computed 一樣會被緩存、依賴值變更了才會重新計算。
    getters: {
        breadCrumb(state) {
            return state.breadCrumb;
        },
        targetProduct(state) {
            return state.targetProduct;
        },
        relevantProducts(state) {
            return state.relevantProducts;
        }
    }
});

export default storeProductInformation;  