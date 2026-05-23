<script setup>
import RangeInput from '@/components/basic/RangeInput.vue';
import SingeOutput from '@/components/basic/SingeOutput.vue';
import BreakEvenService from '@/services/BreakEvenService.vue';
import {ref, computed} from 'vue'

const props = defineProps({
    id: String 
})

const results = ref(null)
const successful = ref(false)

function getCalculationResults(data){
    results.value = data;
    successful.value = true;
}

function formatCurrency(val) {
  if (val == null) return '—'
  return '$' + Number(val).toLocaleString('en-US', { minimumFractionDigits: 0 })
}
 
function formatPct(val) {
  if (val == null) return '—'
  return Number(val).toFixed(1) + '%'
}
 
const maxVal = computed(() => {
  const rev = props.results?.currentRevenue || 0
  const be = props.results?.breakEvenRevenue || 0
  return Math.max(rev, be) * 1.15
})
 
const fillPct = computed(() => {
  const rev = props.results?.currentRevenue || 0
  return Math.min((rev / maxVal.value) * 100, 100)
})
 
const markerPct = computed(() => {
  const be = props.results?.breakEvenRevenue || 0
  return Math.min((be / maxVal.value) * 100, 100)
})

const revenue = ref(0);

function setRevenue(data){
    revenue.value = data;
}
</script>

<template>
  <section class="parent">
 
    <div class="input-container">
      <BreakEvenService
        :sessionId="props.id"
        @calculate="getCalculationResults"
        @revenue = "setRevenue"
      />
    </div>
 
    <div class="output-container" v-if="successful">
 
      <div class="results-card">
        <div class="section-header">
          <h3 class="section-title">Results</h3>
        </div>
 
        <div class="metrics-grid">
          <div class="metric-tile">
            <span class="metric-label">Break-even revenue</span>
            <span class="metric-value">{{ formatCurrency(results.breakEvenRevenue) }}</span>
          </div>
          <div class="metric-tile">
            <span class="metric-label">Drop tolerance</span>
            <span class="metric-value">{{ formatPct(results.dropTolerance) }}</span>
          </div>
          <div class="metric-tile">
            <span class="metric-label">Safety cushion</span>
            <span class="metric-value">{{ formatCurrency(results.cushion) }}</span>
          </div>
        </div>
      </div>
 
      <div class="range-card">
        <div class="range-header">
          <span class="range-title">Revenue vs break-even</span>
          <span class="range-status" :class="statusClass">{{ statusLabel }}</span>
        </div>
 
        <div class="range-track-wrap">
          <div class="range-track">
            <div class="range-fill" :style="{ width: fillPct + '%', background: fillColor }"></div>
            <div class="range-marker" :style="{ left: markerPct + '%' }" :title="'Break-even: ' + formatCurrency(results.breakEvenRevenue)"></div>
          </div>
        </div>
 
        <div class="range-labels">
          <div class="range-label-item">
            <span class="label-dot" style="background: #111827"></span>
            <span class="label-text">Current revenue</span>
            <span class="label-val">{{ formatCurrency(revenue) }}</span>
          </div>
          <div class="range-label-item">
            <span class="label-dot marker-dot"></span>
            <span class="label-text">Break-even</span>
            <span class="label-val">{{ formatCurrency(results.breakEvenRevenue) }}</span>
          </div>
        </div>
      </div>
 
    </div>
  </section>
</template>
 
 
<style scoped>
.parent {
  display: flex;
  flex-direction: column;
  width: 100%;
  height: 100%;
  padding: 30px;
  box-sizing: border-box;
  gap: 22px;
}
 
.input-container,
.output-container {
  display: flex;
  flex-direction: column;
  gap: 22px;
}
 
.results-card,
.range-card {
  display: flex;
  flex-direction: column;
  padding: 20px;
  gap: 16px;
  border-radius: 14px;
  box-sizing: border-box;
  background: #ffffff;
  border: 1px solid #e5e7eb;
  box-shadow: 0 6px 14px rgba(0, 0, 0, 0.05);
}
 
.section-header {
  display: flex;
  align-items: center;
  gap: 8px;
}
 

 
.section-title {
  margin: 0;
}
 
.metrics-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 12px;
}
 
.metric-tile {
  display: flex;
  flex-direction: column;
  gap: 4px;
  background: #f9fafb;
  border-radius: 10px;
  padding: 14px;
}
 
.metric-label {
  font-size: 12px;
  font-weight: 500;
  color: #6b7280;
}
 
.metric-value {
  font-size: 18px;
  font-weight: 600;
  color: #111827;
}
 
.range-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
 
.range-title {
  font-size: 13px;
  font-weight: 600;
  color: #111827;
}
 
.range-status {
  font-size: 12px;
  font-weight: 500;
  padding: 3px 10px;
  border-radius: 999px;
}
 
.status-safe {
  background: #e1f5ee;
  color: #085041;
}
 
.status-warn {
  background: #fef9ec;
  color: #854f0b;
}
 
.status-danger {
  background: #fef2f2;
  color: #991b1b;
}
 
.range-track-wrap {
  padding: 4px 0;
}
 
.range-track {
  position: relative;
  height: 10px;
  background: #f3f4f6;
  border-radius: 999px;
  overflow: visible;
}
 
.range-fill {
  height: 100%;
  border-radius: 999px;
  transition: width 0.4s ease, background 0.3s ease;
}
 
.range-marker {
  position: absolute;
  top: -3px;
  width: 3px;
  height: 16px;
  background: #111827;
  border-radius: 2px;
  transform: translateX(-50%);
  transition: left 0.4s ease;
}
 
.range-labels {
  display: flex;
  justify-content: space-between;
  gap: 12px;
}
 
.range-label-item {
  display: flex;
  align-items: center;
  gap: 6px;
}
 
.label-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  flex-shrink: 0;
}
 
.marker-dot {
  background: #111827;
}
 
.label-text {
  font-size: 12px;
  color: #6b7280;
}
 
.label-val {
  font-size: 12px;
  font-weight: 600;
  color: #111827;
}
</style>