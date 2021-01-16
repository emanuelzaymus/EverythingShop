// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/**
 * Sends /Products/AddProductToCart API request and receives response. Modifies document.
 * @param {number} prodId Product ID
 * @param {string} quantityElemId Html quantity element id
 * @param {number} productPrice Product price
 * @param {string} totalPriceElemId Html total price element id
 */
function addProductToCart(prodId, quantityElemId, productPrice, totalPriceElemId) {
    $.post("/Products/AddProductToCart",
        { productId: prodId },
        function (response) {
            if (response.success) {
                updateElementValue(quantityElemId, response.newProductQuantity);
                updateTotalPrice(totalPriceElemId, productPrice);
            }
        }
    );
}

/**
 * Sends /Products/RemoveProductFromCart API request and receives response. Modifies document.
 * @param {number} prodId Product ID
 * @param {string} quantityElemId Html quantity element id
 * @param {number} productPrice Product price
 * @param {string} totalPriceElemId Html total price element id
 */
function removeProductFromCart(prodId, quantityElemId, productPrice, totalPriceElemId) {
    $.post("/Products/RemoveProductFromCart",
        { productId: prodId },
        function (response) {
            if (response.success) {
                updateElementValue(quantityElemId, response.newProductQuantity);
                updateTotalPrice(totalPriceElemId, -productPrice);
            }
        }
    );
}

/** Private */
function updateElementValue(elemId, newValue) {
    $('#' + elemId).val(newValue);
}

/** Private */
function updateTotalPrice(totalPriceId, productPrice) {
    var totalPriceElem = $('#' + totalPriceId);
    totalPriceElem.html(
        (parseFloat(totalPriceElem.html()) + productPrice)
            .toFixed(2)
    );
}

/**
 * Sends /UserOrders/SetOrderDelivered API request and receives response. Modifies document.
 * @param {string} btnId This button id
 * @param {number} orderId Order ID
 * @param {string} orderBadgeElemId Order html badge element id
 */
function setOrderDelivered(btnId, orderId, orderBadgeElemId) {
    $.post("/UserOrders/SetOrderDelivered",
        { orderId: orderId },
        function (response) {
            updateOrderBadgeSuccess(orderBadgeElemId, response.newOrderState);
            disableElement(btnId)
        }
    );
}

/** Private */
function updateOrderBadgeSuccess(badgeId, newOrderState) {
    $('#' + badgeId).html(newOrderState)
        .removeClass("badge-warning")
        .addClass("badge-success");
}

/**
 * Sends /ManageOrders/SetOrderSent API request and receives response. Modifies document.
 * @param {string} btnId This button id
 * @param {number} orderId Order ID
 * @param {string} orderBadgeElemId Order html badge element id
 */
function setOrderSent(btnId, orderId, orderBadgeElemId) {
    $.post("/ManageOrders/SetOrderSent",
        { orderId: orderId },
        function (response) {
            updateOrderBadgeWarning(orderBadgeElemId, response.newOrderState);
            disableElement(btnId)
        }
    );
}

/** Private */
function updateOrderBadgeWarning(badgeId, newOrderState) {
    $('#' + badgeId).html(newOrderState)
        .removeClass("badge-secondary")
        .addClass("badge-warning");
}

/** Private */
function disableElement(elemId) {
    $('#' + elemId).prop('disabled', true);
}

/** 
 * Goes back to the previous page.
 */
function goToPreviousPage() {
    history.go(-1);
    return false;
}
