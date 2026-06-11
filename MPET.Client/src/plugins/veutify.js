import '@mdi/font/css/materialdesignicons.css' // Import icon fonts
import 'vuetify/styles' // Import baseline Vuetify styles
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const vuetify = createVuetify({
  components,
  directives,
})

export default vuetify