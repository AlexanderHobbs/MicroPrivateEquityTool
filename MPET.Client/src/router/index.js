import { createRouter, createWebHistory } from 'vue-router'

import EarningCalculator from '@/views/EarningCalculator.vue'
import FinancialSummary from '@/views/FinancialSummary.vue'
import LossReport from '@/views/LossReport.vue'

const routes = [
  {
    path: '/earning',
    name: 'EarningCalculator',
    component: EarningCalculator
  },
  {
    path: '/summary',
    name: 'FinancialSummary',
    component: FinancialSummary
  },
  {
    path: '/loss',
    name: 'LossReport',
    component: LossReport
  }
]

export const router = createRouter({
  history: createWebHistory(),
  routes,
})

