// ========================================
// Document Ready
// ========================================
$(document).ready(function () {
    // Initialize all features
    initScrollToTop();
    initPromo();
    initChat();
    setActiveNavLink();

    // Auto dismiss alerts after 5 seconds
    setTimeout(function () {
        $('.alert').fadeOut('slow');
    }, 5000);
});

// ========================================
// Scroll to Top Button
// ========================================
function initScrollToTop() {
    // Create scroll to top button if not exists
    if (!$('.scroll-to-top').length) {
        $('body').append('<div class="scroll-to-top"><i class="fas fa-arrow-up"></i></div>');
    }

    // Show/hide on scroll
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('.scroll-to-top').addClass('show');
        } else {
            $('.scroll-to-top').removeClass('show');
        }
    });

    // Scroll to top on click
    $(document).on('click', '.scroll-to-top', function () {
        $('html, body').animate({ scrollTop: 0 }, 600);
    });
}

// ========================================
// Set Active Nav Link
// ========================================
function setActiveNavLink() {
    const currentPath = window.location.pathname;

    $('.nav-link').each(function () {
        const href = $(this).attr('href');
        if (href && currentPath.includes(href) && href !== '/') {
            $(this).addClass('active');
        }
    });
}

// ========================================
// Promo Popup Functions
// ========================================
function initPromo() {
    // Show promo after 3 seconds on first visit
    const promoShown = sessionStorage.getItem('promoShown');

    if (!promoShown) {
        setTimeout(function () {
            showPromo();
            sessionStorage.setItem('promoShown', 'true');
        }, 3000);
    }

    // Start countdown timer
    startPromoTimer();
}

function showPromo() {
    $('#promoOverlay').fadeIn(300);
    $('#promoPopup').fadeIn(300).css('display', 'block');
    $('body').css('overflow', 'hidden');
}

function closePromo() {
    $('#promoOverlay').fadeOut(300);
    $('#promoPopup').fadeOut(300);
    $('body').css('overflow', 'auto');
}

function startPromoTimer() {
    // Set end time (e.g., 2 hours from now)
    const endTime = new Date().getTime() + (2 * 60 * 60 * 1000);

    const timer = setInterval(function () {
        const now = new Date().getTime();
        const distance = endTime - now;

        if (distance < 0) {
            clearInterval(timer);
            $('#hours, #minutes, #seconds').text('00');
            return;
        }

        const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        const seconds = Math.floor((distance % (1000 * 60)) / 1000);

        $('#hours').text(String(hours).padStart(2, '0'));
        $('#minutes').text(String(minutes).padStart(2, '0'));
        $('#seconds').text(String(seconds).padStart(2, '0'));
    }, 1000);
}

// ========================================
// Chatbox Functions
// ========================================
function initChat() {
    // Pre-defined responses
    window.chatResponses = {
        'xin chào': 'Xin chào! Rất vui được hỗ trợ bạn. Bạn cần giúp gì?',
        'hello': 'Hello! How can I help you today?',
        'giá': 'Bạn có thể xem giá sản phẩm chi tiết trên từng trang sản phẩm. Hoặc cho tôi biết sản phẩm nào bạn quan tâm?',
        'giao hàng': 'Chúng tôi giao hàng toàn quốc. Miễn phí vận chuyển cho đơn hàng trên 1 triệu đồng.',
        'thanh toán': 'Chúng tôi hỗ trợ thanh toán COD, chuyển khoản ngân hàng và ví điện tử.',
        'bảo hành': 'Tất cả sản phẩm đều được bảo hành chính hãng 12 tháng từ nhà sản xuất.',
        'đổi trả': 'Bạn có thể đổi trả trong vòng 7 ngày nếu sản phẩm có lỗi từ nhà sản xuất.',
        'default': 'Cảm ơn bạn đã liên hệ. Bộ phận chăm sóc khách hàng sẽ phản hồi sớm nhất. Hotline: 0909 123 456'
    };
}

function toggleChat() {
    const chatBox = $('#chatBox');
    const isVisible = chatBox.is(':visible');

    if (isVisible) {
        chatBox.fadeOut(300);
    } else {
        chatBox.fadeIn(300);
        $('#chatInput').focus();
    }
}

function sendMessage() {
    const input = $('#chatInput');
    const message = input.val().trim();

    if (message === '') return;

    // Add user message
    addChatMessage(message, 'user');
    input.val('');

    // Simulate bot typing
    setTimeout(function () {
        const response = getBotResponse(message);
        addChatMessage(response, 'bot');
    }, 1000);
}

function handleChatEnter(event) {
    if (event.key === 'Enter') {
        sendMessage();
    }
}

function addChatMessage(message, sender) {
    const chatBody = $('#chatBody');
    const messageClass = sender === 'user' ? 'user' : 'bot';

    const messageHtml = `
        <div class="chat-message ${messageClass}">
            <div class="message-bubble">
                ${message}
            </div>
        </div>
    `;

    chatBody.append(messageHtml);
    chatBody.scrollTop(chatBody[0].scrollHeight);
}

function getBotResponse(message) {
    const lowerMessage = message.toLowerCase();

    // Check for keywords
    for (const [keyword, response] of Object.entries(window.chatResponses)) {
        if (lowerMessage.includes(keyword)) {
            return response;
        }
    }

    return window.chatResponses['default'];
}

// ========================================
// Search Enhancement
// ========================================
$(document).on('focus', '.search-bar input', function () {
    $(this).parent().addClass('search-focused');
});

$(document).on('blur', '.search-bar input', function () {
    $(this).parent().removeClass('search-focused');
});

// ========================================
// Smooth Scroll for Anchor Links
// ========================================
$(document).on('click', 'a[href^="#"]', function (e) {
    const target = $(this.getAttribute('href'));

    if (target.length) {
        e.preventDefault();
        $('html, body').stop().animate({
            scrollTop: target.offset().top - 100
        }, 800);
    }
});

// ========================================
// Image Error Handling
// ========================================
$(document).on('error', 'img', function () {
    if (!$(this).hasClass('error-handled')) {
        $(this).addClass('error-handled');
        const placeholder = $(this).attr('onerror');
        if (placeholder) {
            $(this).attr('src', placeholder.match(/this\.src='(.+?)'/)[1]);
        }
    }
});

// ========================================
// Form Validation Enhancement
// ========================================
$('form').on('submit', function () {
    const submitBtn = $(this).find('button[type="submit"]');
    const originalText = submitBtn.html();

    submitBtn.prop('disabled', true);
    submitBtn.html('<span class="loading"></span> Đang xử lý...');

    setTimeout(function () {
        submitBtn.prop('disabled', false);
        submitBtn.html(originalText);
    }, 3000);
});

// ========================================
// Tooltip Initialization
// ========================================
$(function () {
    $('[data-bs-toggle="tooltip"]').tooltip();
});

// ========================================
// Number Format Helper
// ========================================
function formatNumber(num) {
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

// ========================================
// Copy to Clipboard
// ========================================
function copyToClipboard(text) {
    const textarea = document.createElement('textarea');
    textarea.value = text;
    document.body.appendChild(textarea);
    textarea.select();
    document.execCommand('copy');
    document.body.removeChild(textarea);

    // Show toast notification
    showNotification('Đã copy vào clipboard!', 'success');
}

// ========================================
// Notification System
// ========================================
function showNotification(message, type = 'info') {
    const colors = {
        success: 'linear-gradient(135deg, #10b981 0%, #059669 100%)',
        error: 'linear-gradient(135deg, #ef4444 0%, #dc2626 100%)',
        warning: 'linear-gradient(135deg, #f59e0b 0%, #d97706 100%)',
        info: 'linear-gradient(135deg, #06b6d4 0%, #0891b2 100%)'
    };

    const icons = {
        success: 'fa-check-circle',
        error: 'fa-exclamation-circle',
        warning: 'fa-exclamation-triangle',
        info: 'fa-info-circle'
    };

    const notification = $(`
        <div class="notification-toast" style="background: ${colors[type]}">
            <i class="fas ${icons[type]}"></i>
            <span>${message}</span>
        </div>
    `);

    $('body').append(notification);

    setTimeout(() => {
        notification.addClass('show');
    }, 100);

    setTimeout(() => {
        notification.removeClass('show');
        setTimeout(() => {
            notification.remove();
        }, 300);
    }, 3000);
}

// ========================================
// Loading Overlay
// ========================================
function showLoading() {
    if (!$('.loading-overlay').length) {
        $('body').append(`
            <div class="loading-overlay">
                <div class="loading-spinner">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Loading...</span>
                    </div>
                </div>
            </div>
        `);
    }
    $('.loading-overlay').fadeIn(300);
}

function hideLoading() {
    $('.loading-overlay').fadeOut(300);
}

// ========================================
// Lazy Loading Images
// ========================================
if ('IntersectionObserver' in window) {
    const imageObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                img.src = img.dataset.src;
                img.classList.remove('lazy');
                imageObserver.unobserve(img);
            }
        });
    });

    document.querySelectorAll('img.lazy').forEach(img => {
        imageObserver.observe(img);
    });
}

// ========================================
// Prevent Double Click on Buttons
// ========================================
$(document).on('click', 'button', function () {
    const $btn = $(this);
    if ($btn.data('clicked')) {
        return false;
    }

    $btn.data('clicked', true);
    setTimeout(() => {
        $btn.data('clicked', false);
    }, 2000);
});

// ========================================
// Auto-grow Textarea
// ========================================
$(document).on('input', 'textarea.auto-grow', function () {
    this.style.height = 'auto';
    this.style.height = (this.scrollHeight) + 'px';
});

// ========================================
// Console Welcome Message
// ========================================
console.log('%c🎉 Welcome to QL Kho Hàng! 🎉', 'font-size: 20px; font-weight: bold; color: #667eea;');
console.log('%cBuilt with ❤️ by Your Team', 'font-size: 14px; color: #6b7280;');