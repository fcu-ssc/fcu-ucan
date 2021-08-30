class BulmaModal {
    constructor(selector) {
        this.elem = document.querySelector(selector)
        this.close_data()
    }
    show() {
        this.elem.classList.toggle('is-active')
        this.on_show()
    }
    close() {
        this.elem.classList.toggle('is-active')
        this.on_close()
    }
    close_data() {
        const modalClose = this.elem.querySelectorAll("[data-bulma-modal='close'], .modal-background");
        const that = this;
        modalClose.forEach(function(e) {
            e.addEventListener("click", function() {
                that.elem.classList.toggle('is-active')
                const event = new Event('modal:close');
                that.elem.dispatchEvent(event);
            })
        })
    }
    on_show() {
        const event = new Event('modal:show');
        this.elem.dispatchEvent(event);
    }
    on_close() {
        const event = new Event('modal:close');
        this.elem.dispatchEvent(event);
    }
    addEventListener(event, callback) {
        this.elem.addEventListener(event, callback)
    }
}

let btn = document.querySelector("#delete-btn");
let mdl = new BulmaModal("#delete-modal");

btn.addEventListener("click", function (event) {
    event.preventDefault();
    mdl.show()
})
mdl.addEventListener('modal:show', function() {
    // console.log("opened")
})
mdl.addEventListener("modal:close", function() {
    // console.log("closed")
})