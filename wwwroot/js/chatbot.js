import storeChatBot from './store/chatbot.js';

const chatbot = createApp({
    data() {
        return {
            chat: null,
            chatInput: null,
            chatWindow: null,
            sendChatBtn: null,
            chatToggler: null,
            chatbotCloseBtn: null,
            chatWindowBottomBtn: null,
            chatInputIntiHeight: 0,
            chatWindowBottomBtnIntiBottom: 0,
            chatWindowMaxHeight: 0,

            dialogMessages: [],
            currentUser: "User",
            bottomAnimation: false
        }
    },
    methods: {
        createChat(user, message) {
            this.dialogMessages.push({
                user: user,
                message: message
            });
        },
        sendMessage() {
            let message = this.chatInput.value.trim();
            if (!message) return;
            this.chatInput.value = "";

            this.createChat(this.currentUser, message);
            this.$nextTick(() => {
                this.chatWindow.scrollTo(0, this.chatWindow.scrollHeight);
            });
            this.adjustInput();
            this.getResponse();
        },
        getResponse() {
            let response = "";
            let currentUser = "others";
            response = "May I help you?";
            this.createChat(currentUser, response);
        },
        getChatClass(user) {
            let chatClass = "";
            chatClass = (user === this.currentUser) ? "chat outgoing" : "chat incoming";

            return chatClass;
        },
        getUserIcon(user) {
            let userIcon = null;
            userIcon = (user === this.currentUser) ? false : true;

            return userIcon;
        },
        scrollChatWindow() {
            let chatWindowHeight = this.chatWindow.clientHeight;
            let currentScrollHeight = this.chatWindow.scrollHeight;
            let currentScrollTop = 0;

            if (currentScrollHeight === chatWindowHeight) return;
            setTimeout(() => {
                currentScrollTop = this.chatWindow.scrollTop;
                if (Math.ceil(chatWindowHeight + currentScrollTop) === currentScrollHeight) {
                    if (this.chatWindowBottomBtn.style.display === "none") return;
                    this.bottomAnimation = true;
                    setTimeout(() => {
                        this.chatWindowBottomBtn.style.display = "none";
                        this.bottomAnimation = false;
                    }, 400)
                }
                else {

                    this.chatWindowBottomBtn.style.bottom = `${this.chatWindowBottomBtnIntiBottom}`;
                    this.chatWindowBottomBtn.style.display = "block";
                }
            }, 200)
        },
        turnToBottom() {
            let scrollHeight = this.chatWindow.scrollHeight;
            this.chatWindow.scrollTo(0, scrollHeight);
            this.bottomAnimation = true;
            setTimeout(() => {
                this.chatWindowBottomBtn.style.display = "none";
                this.bottomAnimation = false;
            }, 400);
        },
        adjustInput() {
            if (!this.chatInput.value) {
                this.chatInput.style.height = `${this.chatInputIntiHeight}px`;
                return;
            };
            let distance = this.chatInput.scrollHeight - this.chatInputIntiHeight;

            this.chatInput.style.height = `${this.chatInputIntiHeight}px`;
            this.chatInput.style.height = `${this.chatInput.scrollHeight}px`;

            this.chatWindowBottomBtn.style.bottom = `${this.chatWindowBottomBtnIntiBottom}px`;
            if (this.chatInput.scrollHeight < this.chatWindowMaxHeight) {
                this.chatWindowBottomBtn.style.bottom = `${this.chatWindowBottomBtnIntiBottom + distance}px`;
            }
            else {
                this.chatWindowBottomBtn.style.bottom = `${this.chatWindowMaxHeight + this.chatWindowBottomBtnIntiBottom - this.chatInputIntiHeight}px`;
            }
        },
        clearInput(event) {
            if (event.key === "Enter" && !event.shiftKey) {
                event.preventDefault();
                this.sendMessage();
                this.chatInput.style.height = `${this.chatInputIntiHeight}px`;
                this.chatWindowBottomBtn.style.bottom = `${this.chatWindowBottomBtnIntiBottom}px`;
            }
        },
        closeChat() {
            let chatbot = document.querySelector(".chatbot");
            chatbot.classList.remove("show-chatbot");
        },
        getCssValue(component, propertyValue) {
            let cssValue = null;
            cssValue = window.getComputedStyle(component).getPropertyValue(propertyValue);
            cssValue = Number(cssValue.replace("px", ""));

            return cssValue
        }
    },
    mounted() {
        this.chatInput = this.$refs.chatInput;
        this.chatWindow = this.$refs.chatWindow;
        this.sendChatBtn = this.$refs.sendChatBtn;
        this.chatbotCloseBtn = this.$refs.chatbotCloseBtn;
        this.chatWindowBottomBtn = this.$refs.chatWindowBottomBtn;
        this.chatInputIntiHeight = this.$refs.chatInput.scrollHeight;
        this.chatWindowBottomBtnIntiBottom = this.getCssValue(this.chatWindowBottomBtn, "bottom");
        this.chatWindowMaxHeight = this.getCssValue(this.chatInput, "max-height");

        this.dialogMessages.push({
            user: "others",
            message: "哈囉 👋 \n請問有什麼需要幫助的嗎？"
        });
    }
});
chatbot.use(storeChatBot);
chatbot.mount('.chatbot');

const btnGroup = createApp({
    data() {
        return {
            showChat: false,
            chatbot: null,
            chatbotClassName: "show-chatbot"
        }
    },
    methods: {
        toggleChat() {
            this.$store.commit("toggleChat");
            this.chatbot.classList.toggle(this.chatbotClassName);
        }
    },
    mounted() {
        this.chatbot = document.querySelector(".chatbot");
    }
});

btnGroup.use(storeChatBot);
btnGroup.mount('.btn-group');