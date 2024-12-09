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
        setCart(state, data) {
            state.cart = data;
        },
        adjustCart(state, { method, itemID }) {
            const target = state.cart.cartItems.find(item => item.ID === itemID);                    
            if (method === "-" && target.Quantity > 1) {
                target.Quantity -= 1;
                state.cart.Quantity -= 1;
                state.cart.Subtotal -= target.Price;
            }
            else if (method === "+") {
                target.Quantity += 1;
                state.cart.Quantity += 1;
                state.cart.Subtotal += target.Price;
            }
            else if (method === "remove") {
                const index = state.cart.cartItems.findIndex(item => item.ID === itemID);
                state.cart.cartItems.splice(index, 1);
            }
            else {
                return;
            }
            target.Subtotal = target.Price * target.Quantity;
            state.cart.DeliveryFee = state.cart.Subtotal > 500 ? 0 : 60;
            state.cart.Total = state.cart.Subtotal + state.cart.DeliveryFee;
        },
    },
    // 操作同步或異步事件的處理但不直接修改資料（state）。
    // 是透過commit → 呼叫 mutation 改變 state。
    actions: {
        fetchProgress({ commit, state }) {
            const pathName = window.location.pathname;
            const index = state.paths.indexOf(pathName);
            commit('setProgress');
            commit('adjustProgress', index);
            //console.log(state.currentProgress);
        },
        async fetchCart({ commit }) {
            const url = "/api/Cartapi/GetShoppingCart";    
            try {
                const response = await axios.get(url);                
                commit('setCart', response.data);
                commit('setTableRow');
            } catch (error) {
            }
        },
        async adjustItemQuantity({ commit }, { method, itemID }) {                        
            let controller = ""
            if (method === "-") {
                controller = "DecreaseItem";
            } else if (method === "+") {
                controller = "IncreaseItem";
            } else {
                return;  // quantity <=1;
            }
            const url = `/api/CartApi/${controller}`;        
            const data = { ID: itemID };
            try {
                const response = await axios.put(url, data);                            
                if (response) commit('adjustCart', { method, itemID });
            }
            catch (error) {
            }                                                 
        },
        async removeItem({ commit, state }, { itemID }) {
            const url = "/api/CartApi/RemoveFromCart";
            const data = { ID: itemID };
            try {
                const response = await axios.put(url, data);
                if (response) commit('adjustCart', { method: 'remove', itemID });
            }
            catch (error) {
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
            return state.cart.Quantity || 0;
        },
        subtotal(state) { 
            return state.cart.Subtotal || 0;
        },
        deliveryFee(state) {                                            
            return state.cart.DeliveryFee || 0;
        },
        total(state) {
            return state.cart.Total || 0;
        }
    }
});

export default store;  