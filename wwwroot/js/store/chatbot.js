const storeChatBot = createStore({
    state: {
        showChat: false
    },
    mutations: {
        toggleChat(state) {
            state.showChat = !state.showChat;
        }
    },
    getters: {
        showChatbot(state) {
            return state.showChat;
        }
    }
});

export default storeChatBot;