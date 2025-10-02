// Site.js - Basic JavaScript for the Disaster Alleviation Foundation
// This file provides basic functionality for the application

// Wait for DOM to be ready
document.addEventListener('DOMContentLoaded', function() {
    console.log('Site.js loaded successfully');
    
    // Add any site-wide JavaScript functionality here
    // For now, this is a placeholder file to prevent 404 errors
});

// Basic form validation helper
function validateForm(form) {
    const inputs = form.querySelectorAll('input[required]');
    let isValid = true;
    
    inputs.forEach(input => {
        if (!input.value.trim()) {
            isValid = false;
            input.classList.add('is-invalid');
        } else {
            input.classList.remove('is-invalid');
        }
    });
    
    return isValid;
}
