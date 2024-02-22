/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./Features/**/*.{js,ts,css,cshtml}",
    "./Pages/**/*.{js,ts,css,cshtml}",
  ],
  theme: {
    extend: {
      colors: {
        sakg: '#009578'
      },
      screens: {
        '3xl': '1792px'
      },
      minWidth: {
        "px": "1px",
        "0.5": "0.125rem", /* 2px */
        "1": "0.25rem",
        "1.5": "0.375rem", /* 6px */
        "2": "0.5rem", /* 8px */
        "2.5": "0.625rem", /* 10px */
        "3": "0.75rem", /* 12px */
        "3.5": "0.875rem", /* 14px */
        "4": "1rem", /* 16px */
        "5": "1.25rem", /* 20px */
        "6": "1.5rem", /* 24px */
        "7": "1.75rem", /* 28px */
        "8": "2rem", /* 32px */
        "9": "2.25rem", /* 36px */
        "10": "2.5rem", /* 40px */
        "11": "2.75rem", /* 44px */
        "12": "3rem", /* 48px */
        "14": "3.5rem", /* 56px */
        "16": "4rem", /* 64px */
        "20": "5rem", /* 80px */
        "24": "6rem", /* 96px */
        "28": "7rem", /* 112px */
        "32": "8rem", /* 128px */
        "36": "9rem", /* 144px */
        "40": "10rem", /* 160px */
        "44": "11rem", /* 176px */
        "48": "12rem", /* 192px */
        "52": "13rem", /* 208px */
        "56": "14rem", /* 224px */
        "60": "15rem", /* 240px */
        "64": "16rem", /* 256px */
        "72": "18rem", /* 288px */
        "80": "20rem", /* 320px */
        "96": "24rem", /* 384px */
        "fit": "fit-content",
        "min": "min-content"
      },
      maxWidth: {
        "px": "1px",
        "0.5": "0.125rem", /* 2px */
        "1": "0.25rem",
        "1.5": "0.375rem", /* 6px */
        "2": "0.5rem", /* 8px */
        "2.5": "0.625rem", /* 10px */
        "3": "0.75rem", /* 12px */
        "3.5": "0.875rem", /* 14px */
        "4": "1rem", /* 16px */
        "5": "1.25rem", /* 20px */
        "6": "1.5rem", /* 24px */
        "7": "1.75rem", /* 28px */
        "8": "2rem", /* 32px */
        "9": "2.25rem", /* 36px */
        "10": "2.5rem", /* 40px */
        "11": "2.75rem", /* 44px */
        "12": "3rem", /* 48px */
        "14": "3.5rem", /* 56px */
        "16": "4rem", /* 64px */
        "20": "5rem", /* 80px */
        "24": "6rem", /* 96px */
        "28": "7rem", /* 112px */
        "32": "8rem", /* 128px */
        "36": "9rem", /* 144px */
        "40": "10rem", /* 160px */
        "44": "11rem", /* 176px */
        "48": "12rem", /* 192px */
        "52": "13rem", /* 208px */
        "56": "14rem", /* 224px */
        "60": "15rem", /* 240px */
        "64": "16rem", /* 256px */
        "72": "18rem", /* 288px */
        "80": "20rem", /* 320px */
        "96": "24rem", /* 384px */
        "fit": "fit-content",
        "min": "min-content"
      }
    },
  },
  plugins: [require("daisyui")],

  daisyui: {
    themes: [
      {
        sakg: {
          "primary": "#009578",
          "primary-content": "#ffffff",
          "secondary": "#60b53c",
          "secondary-content": "#ffffff",
          "accent": "#12767b",
          "neutral": "#3d4451",
          "base-100": "#ffffff",
          "success": "#28a745",
          "success-content": "#ffffff",
          "warning": "#ffc107",
          "warning-content": "#ffffff",
          "error": "#dc3545",
          "error-content": "#ffffff",
          "info": "#17a2b8",
          "info-content": "#000000",
        },
      }
    ], // false: only light + dark | true: all themes | array: specific themes like this ["light", "dark", "cupcake"]
    darkTheme: "false", // name of one of the included themes for dark mode
    base: true, // applies background color and foreground color for root element by default
    styled: true, // include daisyUI colors and design decisions for all components
    utils: true, // adds responsive and modifier utility classes
    prefix: "", // prefix for daisyUI classnames (components, modifiers and responsive class names. Not colors)
    logs: true, // Shows info about daisyUI version and used config in the console when building your CSS
    themeRoot: ":root", // The element that receives theme color CSS variables
  },
}

