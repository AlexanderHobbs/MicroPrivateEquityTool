<script setup>
import DSCRService from '@/services/DSCRService.vue';
import { ref, onMounted, watch, nextTick } from 'vue';
import { Chart, ArcElement, DoughnutController, Tooltip } from 'chart.js';

Chart.register(ArcElement, DoughnutController, Tooltip);

const prop = defineProps({
    id: String 
})

const calculationResults = ref(null)
const successful = ref(false)
let gaugeChart = null

function DscrResults(data) {
    calculationResults.value = data;
    successful.value = true;

    let style = null;

    if(calculationResults.warningLevel == 0){
        style = "red"
    }else if (calculationResults.warningLevel == 1){
        style = "yellow"
    }else{
        style = "green"
    }
    nextTick(() => renderGauge(data.dscrRatio, style));
}

function renderGauge(value, style) {
    const ctx = document.getElementById('dscrGauge');
    if (!ctx) return;
    if (gaugeChart) gaugeChart.destroy();

    const clamped = Math.min(Math.max(value, 0), 3);
    const remaining = 3 - clamped;

    gaugeChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            datasets: [{
                data: [clamped, remaining],
                backgroundColor: [style, '#e5e7eb'],
                borderWidth: 0,
                circumference: 180,
                rotation: 270,
            }]
        },
        options: {
            responsive: true,
            cutout: '75%',
            plugins: { tooltip: { enabled: false } },
            animation: { duration: 800, easing: 'easeInOutQuart' }
        }
    });
}

const warningConfig = {
    0: { label: 'High Risk',  color: '#ef4444' },
    1: { label: 'Caution',    color: '#f59e0b' },
    2: { label: 'Healthy',    color: '#22c55e' },
}

function getWarning(level) {
    return warningConfig[level] ?? { label: 'Unknown', color: '#9ca3af' };
}

function formatCash(val) {
    if (val == null) return '—';
    return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0 }).format(val);
}
</script>

<template>
    <div class="parent">

        <div class="input">
            <DSCRService :sessionId="prop.id" @calculate="DscrResults" />
        </div>

        <div class="output" v-if="successful">

            <!-- DSCR Ratio Gauge -->
            <div class="card">
                <p class="card-label">DSCR Ratio</p>
                <div class="gauge-wrapper">
                    <canvas id="dscrGauge"></canvas>
                    <div class="gauge-center">
                        <span class="gauge-value"
                        :style="{ color: getWarning(calculationResults.warningLevel).color  }"
                        >{{ (calculationResults.dscrRatio * 10).toFixed(2) }}x</span>
                    </div>
                </div>
                <p class="slider-hint">0 — 3x range</p>
            </div>

            <!-- Remaining Cash Flow -->
            <div class="card">
                <p class="card-label">Remaining Cash Flow</p>
                <p class="card-value">{{ formatCash(calculationResults.remainingCashFlow) }}</p>
            </div>

            <!-- Warning Level -->
            <div class="card">
                <p class="card-label">Warning Level</p>
                <div class="badge" :style="{ backgroundColor: getWarning(calculationResults.warningLevel).color }">
                    {{ getWarning(calculationResults.warningLevel).label }}
                </div>
            </div>

        </div>
    </div>
</template>

<style scoped>
.parent {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100%;
    box-sizing: border-box;
    padding: 30px;
}

.input {
    width: 100%;
    box-sizing: border-box;
}

.output {
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    width: 100%;
    gap: 20px;
    padding: 40px 20px;
    border-radius: 14px;
    box-sizing: border-box;
    background: rgba(255, 255, 255);
    border: 1px solid #e5e7eb;
    box-shadow: 0 6px 14px rgba(0, 0, 0, 0.05);
    margin-top: 50px;
}

/* Cards */
.card {
    flex: 1 1 200px;
    display: flex;
    flex-direction: column;
    align-items: center;
    background: #fafafa;
    border: 1px solid #e5e7eb;
    border-radius: 12px;
    padding: 24px 16px;
    gap: 12px;
}

.card-label {
    font-size: 0.75rem;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 0.08em;
    color: #6b7280;
    margin: 0;
}

.card-value {
    font-size: 1.6rem;
    font-weight: 700;
    color: #111827;
    margin: 0;
}

/* Gauge */
.gauge-wrapper {
    position: relative;
    display: flex;
    justify-content: center;
    width: 180px;
    height: 90px;
    overflow: hidden;
}

.gauge-center {
    position: absolute;
    bottom: 0;
    left: 50%;
    transform: translateX(-50%);
    text-align: center;
}

.gauge-value {
    font-size: 1.4rem;
    font-weight: 700;
    color: #000000;
}


.slider-hint {
    font-size: 0.7rem;
    color: #9ca3af;
    margin: 0;
}

/* Warning Badge */
.badge {
    padding: 8px 20px;
    border-radius: 999px;
    color: white;
    font-weight: 700;
    font-size: 0.9rem;
    letter-spacing: 0.03em;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.15);
}
</style>