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
            updateOrderBadge(orderBadgeElemId, newOrderState);
            disableElement(btnId)
        }
    );
}

function updateOrderBadge(badgeId, newOrderState) {
    $('#' + badgeId).html(newOrderState)
        .removeClass("badge-warning")
        .addClass("badge-success");
}

function disableElement(elemId) {
    $('#' + elemId).prop('disabled', true);
}