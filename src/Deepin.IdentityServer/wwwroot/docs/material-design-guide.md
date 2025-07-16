# Deepin Material Design System - Style Guide

这是基于Angular Material Design理念为Deepin Identity Server创建的统一设计系统。

## 🎨 Design Principles

### Material Design 核心原理
- **Material as Metaphor**: 使用物理世界的隐喻来创建更熟悉的用户界面
- **Bold, Graphic, Intentional**: 大胆的设计选择，清晰的视觉层次
- **Motion Provides Meaning**: 有意义的动画和过渡效果

### 设计原则
1. **一致性优先**: 所有组件遵循统一的设计语言
2. **可访问性**: 支持键盘导航、屏幕阅读器、色彩对比度
3. **响应式设计**: 适配各种屏幕尺寸
4. **性能优化**: 优雅降级和渐进增强

## 🎨 Color Palette

### Primary Colors
- **Primary**: `#1976d2` (蓝色)
- **Primary Light**: `#42a5f5` 
- **Primary Dark**: `#1565c0`

### Secondary Colors  
- **Accent**: `#ff4081` (粉色)
- **Success**: `#4caf50` (绿色)
- **Warning**: `#ff9800` (橙色)
- **Error**: `#f44336` (红色)

### Neutral Colors
- **Background**: `#fafafa`
- **Surface**: `#ffffff`
- **On Surface**: `#212121`
- **On Surface Variant**: `#757575`

## 📦 Component Library

### 1. Navigation

```html
<nav class="navbar navbar-expand-lg mat-navbar">
    <a href="~/" class="navbar-brand">
        <span class="material-icons">account_circle</span>
        Deepin Account
    </a>
</nav>
```

### 2. Cards

```html
<!-- Basic Card -->
<div class="card mat-card">
    <div class="card-header">
        <h2>Card Title</h2>
    </div>
    <div class="card-body">
        Card content goes here
    </div>
</div>

<!-- Elevated Card -->
<div class="card mat-card mat-card-elevated">
    <!-- Content -->
</div>
```

### 3. Buttons

```html
<!-- Primary Button -->
<button class="btn mat-button mat-button-primary">
    <span class="material-icons me-1">login</span>
    Sign In
</button>

<!-- Secondary Button -->
<button class="btn mat-button mat-button-secondary">
    Cancel
</button>

<!-- Outline Button -->
<button class="btn mat-button mat-button-outline">
    Learn More
</button>

<!-- Text Button -->
<button class="btn mat-button mat-button-text">
    Forgot Password?
</button>

<!-- Floating Action Button -->
<button class="btn mat-fab">
    <span class="material-icons">add</span>
</button>
```

### 4. Form Fields

```html
<div class="mat-form-field">
    <label class="form-label">Username</label>
    <input class="form-control" placeholder="Enter username" type="text">
</div>

<!-- Checkbox -->
<div class="mat-checkbox">
    <input class="form-check-input" type="checkbox" id="remember">
    <label class="form-check-label" for="remember">
        Remember me
    </label>
</div>
```

### 5. Alerts

```html
<!-- Success Alert -->
<div class="alert mat-alert mat-alert-success">
    <span class="material-icons me-2">check_circle</span>
    Operation completed successfully!
</div>

<!-- Error Alert -->
<div class="alert mat-alert mat-alert-danger">
    <span class="material-icons me-2">error</span>
    An error occurred. Please try again.
</div>

<!-- Warning Alert -->
<div class="alert mat-alert mat-alert-warning">
    <span class="material-icons me-2">warning</span>
    Please review your information.
</div>

<!-- Info Alert -->
<div class="alert mat-alert mat-alert-info">
    <span class="material-icons me-2">info</span>
    Additional information available.
</div>
```

### 6. Lists

```html
<div class="mat-list">
    <a href="#" class="mat-list-item">
        <span class="material-icons me-3">home</span>
        <div>
            <strong>Home</strong>
            <div class="mat-text-muted">Go to homepage</div>
        </div>
    </a>
    <a href="#" class="mat-list-item">
        <span class="material-icons me-3">settings</span>
        <div>
            <strong>Settings</strong>
            <div class="mat-text-muted">Manage your account</div>
        </div>
    </a>
</div>
```

## 📏 Layout System

### Page Structure

```html
<div class="page-name">
    <!-- Page Header -->
    <div class="mat-page-header">
        <h1>Page Title</h1>
        <p class="lead">Page description</p>
    </div>
    
    <!-- Page Content -->
    <div class="mat-content">
        <div class="row justify-content-center">
            <div class="col-lg-6">
                <!-- Content -->
            </div>
        </div>
    </div>
</div>
```

### Spacing System

使用CSS变量进行一致的间距：

- `--spacing-xs`: 4px
- `--spacing-sm`: 8px  
- `--spacing-md`: 16px
- `--spacing-lg`: 24px
- `--spacing-xl`: 32px
- `--spacing-xxl`: 48px

## 🎭 Elevation System

Material Design使用阴影来表示元素的层次：

```html
<div class="mat-elevation-0">No shadow</div>
<div class="mat-elevation-1">Light shadow</div>
<div class="mat-elevation-2">Medium shadow</div>
<div class="mat-elevation-3">Strong shadow</div>
<div class="mat-elevation-4">Very strong shadow</div>
```

## 🔤 Typography

使用Roboto字体系列提供清晰的文本层次：

```html
<h1>Display Large (2.5rem)</h1>
<h2>Display Medium (2rem)</h2>
<h3>Display Small (1.5rem)</h3>
<p>Body text (1rem)</p>
<small class="mat-text-muted">Caption text (0.875rem)</small>
```

## 🎨 Utility Classes

### Text Colors
- `.mat-text-primary`: 主色调文本
- `.mat-text-accent`: 强调色文本  
- `.mat-text-warn`: 警告色文本
- `.mat-text-muted`: 次要文本

### Background Colors
- `.mat-bg-primary`: 主色调背景
- `.mat-bg-accent`: 强调色背景
- `.mat-bg-surface`: 表面背景

## 📱 Responsive Breakpoints

```css
/* Mobile First Approach */
@media (max-width: 768px) {
    /* Mobile styles */
}

@media (min-width: 769px) and (max-width: 1024px) {
    /* Tablet styles */
}

@media (min-width: 1025px) {
    /* Desktop styles */
}
```

## ♿ Accessibility Features

### Built-in Accessibility
1. **Focus Management**: 明确的焦点指示器
2. **Keyboard Navigation**: 完整的键盘支持
3. **Screen Reader Support**: 语义化HTML和ARIA标签
4. **Color Contrast**: WCAG AA级别的对比度
5. **Touch Targets**: 最小44px的触摸目标

### ARIA Labels
Material Icons会自动添加适当的aria-label：

```html
<button aria-label="Sign in">
    <span class="material-icons">login</span>
</button>
```

## 🎬 Animation & Motion

### CSS Transitions
使用预定义的缓动函数：

- `--transition-fast`: 150ms cubic-bezier(0.4, 0, 0.2, 1)
- `--transition-standard`: 300ms cubic-bezier(0.4, 0, 0.2, 1)
- `--transition-slow`: 500ms cubic-bezier(0.4, 0, 0.2, 1)

### Interactive Effects
- **Ripple Effect**: 按钮点击波纹效果
- **Hover States**: 鼠标悬停状态变化
- **Focus States**: 键盘焦点指示

## 🌙 Dark Mode Support

设计系统包含深色主题支持：

```css
@media (prefers-color-scheme: dark) {
    /* 深色主题变量会自动应用 */
}
```

## 🔧 Customization

### CSS Custom Properties

可以通过修改CSS变量来自定义主题：

```css
:root {
    --primary: #your-color;
    --border-radius-md: your-radius;
    /* 其他变量 */
}
```

## 📋 Best Practices

### Do's ✅
- 始终使用Material Design组件类
- 保持一致的间距和对齐
- 使用Material Icons进行图标
- 遵循颜色层次系统
- 确保充足的触摸目标大小

### Don'ts ❌ 
- 不要混合使用不同的设计系统
- 不要忽略可访问性要求
- 不要在小屏幕上使用过小的触摸目标
- 不要使用过多的阴影层级
- 不要破坏响应式布局

## 🚀 Implementation Examples

参考以下页面查看完整的实现示例：

1. **Login Page** (`/Account/Login`): 表单设计和验证
2. **Registration** (`/Account/Create`): 多步骤表单
3. **Home Page** (`/`): 列表和导航
4. **Logout** (`/Account/Logout`): 确认对话框

## 📚 Resources

- [Material Design Guidelines](https://material.io/design)
- [Google Material Icons](https://fonts.google.com/icons)
- [Bootstrap Documentation](https://getbootstrap.com/)
- [WCAG Accessibility Guidelines](https://www.w3.org/WAI/WCAG21/quickref/)

---

**版本**: 1.0  
**更新日期**: 2025年7月  
**维护者**: Deepin开发团队
