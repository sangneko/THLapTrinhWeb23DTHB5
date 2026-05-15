const productsData = [
    { id: 'SP001', name: 'MacBook Pro M3 Max', price: 85000000, img: 'https://picsum.photos/id/1/400/300', category: 'laptop', badge: 'HOT', desc: 'Cỗ máy di động mạnh nhất cho Developer HUTECH.' },
    { id: 'SP002', name: 'iPhone 16 Pro Max', price: 34990000, img: 'https://picsum.photos/id/160/400/300', category: 'phone', badge: 'NEW', desc: 'Chất liệu Titanium, chip A18 Pro siêu tốc.' },
    { id: 'SP003', name: 'VGA ASUS ROG RTX 5090', price: 60500000, img: 'https://picsum.photos/id/2/400/300', category: 'vga', badge: 'SALE', desc: 'Chiến mọi game AAA, xử lý tác vụ AI mượt mà.' },
    { id: 'SP004', name: 'Màn hình Dell UltraSharp 32"', price: 22000000, img: 'https://picsum.photos/id/3/400/300', category: 'monitor', badge: '', desc: 'Màu sắc chuẩn đồ họa, độ phân giải 4K.' }
];

function renderProducts(dataList) {
    const productGrid = document.getElementById('productGrid');
    if (!productGrid) return;
    productGrid.innerHTML = '';
    dataList.forEach(item => {
        let badgeHTML = item.badge ? `<div class="badge ${item.badge === 'SALE' ? 'sale' : 'hot'}">${item.badge}</div>` : '';
        productGrid.innerHTML += `
            <div class="product-card">
                ${badgeHTML}
                <a href="detail.html?id=${item.id}">
                    <div class="product-img"><img src="${item.img}" alt="${item.name}"></div>
                </a>
                <div class="product-info">
                    <a href="detail.html?id=${item.id}"><h3>${item.name}</h3></a>
                    <p class="desc">${item.desc}</p>
                    <div class="price-action">
                        <span class="price">${item.price.toLocaleString('vi-VN')}đ</span>
                        <button class="add-cart" onclick="addToCart('${item.id}')"><i class="fa-solid fa-cart-plus"></i></button>
                    </div>
                </div>
            </div>`;
    });
}

function renderDetail() {
    const detailName = document.getElementById('detailName');
    if (!detailName) return;

    const urlParams = new URLSearchParams(window.location.search);
    const productId = urlParams.get('id');
    const product = productsData.find(p => p.id === productId);

    if (product) {
        document.getElementById('currentImage').src = product.img;
        detailName.innerText = product.name;
        document.getElementById('detailPrice').innerText = product.price.toLocaleString('vi-VN') + 'đ';
        document.getElementById('detailDesc').innerText = product.desc;
        document.getElementById('breadcrumbName').innerText = product.name;
        document.getElementById('detailAddToCartBtn').setAttribute('onclick', `window.addToCart('${product.id}')`);
    }
}

let cart = JSON.parse(localStorage.getItem('TECH_CART')) || [];
function saveCart() { localStorage.setItem('TECH_CART', JSON.stringify(cart)); }

function updateCartUI() {
    const cartBody = document.getElementById('cartContent');
    const cartBadge = document.querySelector('.cart-badge');
    const cartTotal = document.getElementById('cartTotal');
    if (!cartBody) return;

    cartBody.innerHTML = '';
    let total = 0;
    cart.forEach((item, index) => {
        total += item.price * item.qty;
        cartBody.innerHTML += `
            <div class="cart-item">
                <img src="${item.img}" width="50">
                <div>
                    <h6>${item.name}</h6>
                    <p>${item.qty} x ${item.price.toLocaleString('vi-VN')}đ</p>
                </div>
                <i class="fa-solid fa-trash" onclick="removeItem(${index})"></i>
            </div>`;
    });
    if (cartBadge) cartBadge.innerText = cart.length;
    if (cartTotal) cartTotal.innerText = `Tổng: ${total.toLocaleString('vi-VN')}đ`;
}

window.addToCart = function(id, qty = 1) {
    const product = productsData.find(p => p.id === id);
    let item = cart.find(i => i.id === id);
    if (item) item.qty += qty;
    else cart.push({ ...product, qty });
    saveCart(); updateCartUI();
    document.getElementById('cartSidebar').classList.add('active');
    document.getElementById('cartOverlay').classList.add('active');
}

window.removeItem = function(index) {
    cart.splice(index, 1);
    saveCart(); updateCartUI();
}

document.addEventListener('DOMContentLoaded', () => {
    if (document.getElementById('productGrid')) renderProducts(productsData);
    renderDetail();
    updateCartUI();
    const toggle = document.getElementById('cartToggle'), close = document.getElementById('closeCart'), 
          sidebar = document.getElementById('cartSidebar'), overlay = document.getElementById('cartOverlay');
    const action = () => { sidebar.classList.toggle('active'); overlay.classList.toggle('active'); };
    if (toggle) toggle.onclick = action; if (close) close.onclick = action; if (overlay) overlay.onclick = action;
});