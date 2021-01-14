// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function addProductToCart(prodId, quantityElemId) {
    $.post("/Products/AddProductToCart",
        { productId: prodId },
        function (newProductQuantity) {
            updateElementValue(quantityElemId, newProductQuantity);
        }
    );
}

function removeProductFromCart(prodId, quantityElemId) {
    $.post("/Products/RemoveProductFromCart",
        { productId: prodId },
        function (newProductQuantity) {
            updateElementValue(quantityElemId, newProductQuantity);
        }
    );
}

function updateElementValue(elemId, newValue) {
    $('#' + elemId).val(newValue);
}

function setOrderDelivered(btnId, orderId, orderBadgeElemId) {
    $.post("/UserOrders/SetOrderDelivered",
        { orderId: orderId },
        function (newOrderState) {
            updateOrderBadgeSuccess(orderBadgeElemId, newOrderState);
            disableElement(btnId)
        }
    );
}

function updateOrderBadgeSuccess(badgeId, newOrderState) {
    $('#' + badgeId).html(newOrderState)
        .removeClass("badge-warning")
        .addClass("badge-success");
}

function setOrderSent(btnId, orderId, orderBadgeElemId) {
    $.post("/ManageOrders/SetOrderSent",
        { orderId: orderId },
        function (newOrderState) {
            updateOrderBadgeWarning(orderBadgeElemId, newOrderState);
            disableElement(btnId)
        }
    );
}

function updateOrderBadgeWarning(badgeId, newOrderState) {
    $('#' + badgeId).html(newOrderState)
        .removeClass("badge-secondary")
        .addClass("badge-warning");
}

function disableElement(elemId) {
    $('#' + elemId).prop('disabled', true);
}

function goToPreviousPage() {
    history.go(-1);
    return false;
}
