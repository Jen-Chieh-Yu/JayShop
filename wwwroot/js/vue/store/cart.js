const store = createStore({
    // 響應式的資料狀態儲存, 資料狀態變化時，有用到的 component 都會即時更新。
    state: {
        // 進度條
        paths: ["/cart/cart", "/order/order", "/finishorder/finishorder"],
        currentProgress: [],
        steps: [
            { label: "購物車", icon: "bi bi-cart4" },
            { label: "訂單確認", icon: "bi bi-file-earmark-text-fill" },
            { label: "訂單送出", icon: "bi bi-file-earmark-text-fill" },
        ],
        // 購物車列表
        tableRow: {
            colClass: [],
            colName: []
        },
        cart: {
            cartItems: [],
            quantity: 0,
            subtotal: 0,
            deliveryFee: 0,
            total: 0
        }
    },
    // 改變state。
    // 只處理同步函數：不要在這進行非同步的動作（例如 setTimeout / 打 API 取遠端資料...等）。
    mutations: {
        setProgress(state) {
            const arrayLength = state.paths.length;
            const falseArray = Array(arrayLength).fill(false);
            state.currentProgress = falseArray;
            //console.log(state.currentProgress);
        },
        adjustProgress(state, length) {
            for (let i = 0; i <= length; i++) {
                state.currentProgress[i] = true;
            }
            //console.log(state.currentProgress);
        },
        setTableRow(state) {
            const currentPath = window.location.pathname;
            //console.log(currentPath);
            switch (currentPath) {
                case state.paths[0]:
                    state.tableRow = {
                        colClass: ["col-sm-2", "col-sm-4", "col-sm-1", "col-sm-3", "col-sm-1", "col-sm-1"],
                        colName: ["商品圖片", "商品名稱", "單價", "數量", "小計", ""]
                    }
                    break;
                case state.paths[1]:
                    state.tableRow = {
                        colClass: ["col-sm-2", "col-sm-4", "col-sm-1", "col-sm-3", "col-sm-2"],
                        colName: ["商品圖片", "商品名稱", "單價", "數量", "小計"]
                    }
                    break;
                case state.paths[2]:
                    break;
                default:
                    return;
            }
            //console.log(state.tableRow);
        },
        setCart(state, currentCart) {
            Object.assign(state.cart, currentCart);
        },
        adjustCart(state, { method, itemID }) {
            const target = state.cart.cartItems.find(item => item.id === itemID);
                                                                          
            if (method === "-" && target.quantity > 1) {
                target.quantity -= 1;
                state.cart.quantity -= 1;
                state.cart.subtotal -= target.price;
            }
            else if (method === "+") {
                target.quantity += 1;
                state.cart.quantity += 1;
                state.cart.subtotal += target.price;
            }
            else if (method === "delete") {                                                                                 
                const index = state.cart.cartItems.findIndex(item => item.id === itemID);
                state.cart.cartItems.splice(index, 1);
            }
            else {
                return;
            }                                     
            target.subtotal = target.price * target.quantity;
            state.cart.deliveryFee = state.cart.subtotal > 500 ? 0 : 60;
            state.cart.total = state.cart.subtotal + state.cart.deliveryFee;
        },
    },
    // 操作同步或異步事件的處理但不直接修改資料（state）。
    // 是透過commit → 呼叫 mutation 改變 state。
    actions: {
        getProgressAPI({ commit, state }) {
            const pathName = window.location.pathname;
            const index = state.paths.indexOf(pathName);
            commit('setProgress');
            commit('adjustProgress', index);
            //console.log(state.currentProgress);
        },
        async getCartAPI({ commit }) {
            const apiURL = "/api/Cartapi/GetShoppingCart";
            try {
                const response = await axios.get(apiURL);
                //console.log(response);
                if (response.status === 200 && response.data.currentCart != null) {
                    commit('setCart', response.data.currentCart);
                    commit('setTableRow');
                }
            } catch (error) {
                console.log("API Error : ", error);
            }
        },
        async adjustItemQuantityAPI({ commit }, { method, itemID }) {
            let controller = ""
            if (method === "-") {
                controller = "DecreaseItem";
            } else if (method === "+") {
                controller = "IncreaseItem";
            } else {
                return;  // quantity <=1;
            }                                                       
            const apiURL = `/api/CartApi/${controller}`;
            const data = { ID: itemID };
            try {
                const response = await axios.put(apiURL, data);
                //console.log(response);
                if (response.status === 200 && response.data.success === true) { 
                    commit('adjustCart', { method, itemID });
                }
            }
            catch (error) {
                console.log("API Error : ", error);
            }
        },
        async deleteItemAPI({ commit, state }, { itemID }) {
            const apiURL = "/api/CartApi/DeleteFromCart";
            const data = { ID: itemID };
            try {
                const response = await axios.put(apiURL, data);
                if (response.status === 200 && response.data.success === true) {
                    commit('adjustCart', { method: 'delete', itemID });
                }
            }
            catch (error) {
                console.log("API Error : ", error);
            };
        }
    },
    // 加工資料呈現。
    // 同 computed 一樣會被緩存、依賴值變更了才會重新計算。
    getters: {
        steps(state) {
            return state.steps;
        },
        currentProgress(state) {
            return state.currentProgress;
        },
        tableRow(state) {
            return state.tableRow;
        },
        cart(state) {
            return state.cart || null;
        },
        cartItems(state) {
            return state.cart.cartItems || null;
        },
        quantity(state) {
            return state.cart.quantity || 0;
        },
        subtotal(state) {
            return state.cart.subtotal || 0;
        },
        deliveryFee(state) {
            return state.cart.deliveryFee || 0;
        },
        total(state) {
            return state.cart.total || 0;
        }
    }
});

export default store;  