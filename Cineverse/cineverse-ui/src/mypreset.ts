import { definePreset } from '@primeuix/themes';
import Aura from '@primeuix/themes/aura';

// TODO: Create a beautiful preset
const MyPreset = definePreset(Aura, {
  semantic: {
    primary: {
      50: '{slate.100}',
      100: '{slate.100}',
      200: '{slate.200}',
      300: '{slate.300}',
      400: '{slate.400}',
      500: '{slate.700}',
      600: '{slate.900}',
      700: '{slate.1000}',
      800: '{slate.800}',
      900: '{slate.900}',
      950: '{slate.950}',
    },
  },
});

export default MyPreset;
