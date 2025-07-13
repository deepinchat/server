/**
 * Material Design CSS Styles for Deepin Identity Server
 * Pure CSS implementation without JavaScript interference
 */

// CSS for Material Design components
const materialCSS = `
/* Material Design Button Styles */
.mat-button, .mat-fab {
    position: relative;
    overflow: hidden;
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.mat-button:hover {
    transform: translateY(-1px);
    box-shadow: 0 4px 8px rgba(0,0,0,0.12);
}

.mat-button:active {
    transform: translateY(0);
}

/* Form Field Focus States */
.mat-form-field .form-control:focus {
    border-color: var(--primary) !important;
    box-shadow: 0 0 0 0.2rem rgba(25, 118, 210, 0.25) !important;
}

/* Enhanced Focus Styles */
.mat-button:focus-visible {
    outline: 2px solid var(--primary);
    outline-offset: 2px;
}

.form-control:focus-visible {
    outline: none;
}

/* Loading spinner styles */
.spinner-border-sm {
    width: 1rem;
    height: 1rem;
    border-width: 0.1em;
}

.spinner-border {
    display: inline-block;
    vertical-align: -0.125em;
    border: 0.25em solid currentColor;
    border-right-color: transparent;
    border-radius: 50%;
    animation: spinner-border 0.75s linear infinite;
}

@keyframes spinner-border {
    to {
        transform: rotate(360deg);
    }
}

/* Button disabled state */
.mat-button:disabled {
    opacity: 0.6;
    cursor: not-allowed;
    transform: none !important;
}

.mat-button:disabled:hover {
    transform: none !important;
    box-shadow: var(--shadow-1) !important;
}

/* Simple hover effects for better UX */
.mat-list-item:hover {
    background-color: var(--outline-variant);
    transition: background-color 0.2s ease;
}

.mat-card:hover {
    box-shadow: var(--shadow-2);
    transition: box-shadow 0.3s ease;
}

/* Smooth transitions for form elements */
.mat-form-field .form-control {
    transition: border-color 0.2s ease, box-shadow 0.2s ease;
}

/* Accessibility improvements */
*:focus-visible {
    outline: 2px solid var(--primary);
    outline-offset: 2px;
}

/* Remove default focus outline for mouse users */
*:focus:not(:focus-visible) {
    outline: none;
}
`;

// Inject CSS styles only
const style = document.createElement('style');
style.textContent = materialCSS;
document.head.appendChild(style);
