<script setup>
import { ref } from 'vue';

const props = defineProps({ sessionId: String });

// Scenario store - up to 4 scenarios can be saved and compared side-by-side
const scenarios = ref([]);
const scenarioName = ref('');

// Form state for a new scenario
const form = ref({
  name: '',
  purchasePrice: '',
  sdeMultiple: '',
  equityInjection: '',
  sbaRate: '',
  sbaTerm: '',
  sellerNoteAmount: '',
  sellerNoteRate: '',
  projectedRevenue: '',
  projectedMargin: '',
});

const formVisible = ref(false);
const editIndex = ref(null);

function openForm(index = null) {
  if (index !== null) {
    // Editing existing scenario
    const s = scenarios.value[index];
    Object.assign(form.value, s);
    editIndex.value = index;
  } else {
    // New scenario
    Object.keys(form.value).forEach(k => form.value[k] = '');
    editIndex.value = null;
  }
  formVisible.value = true;
}

function saveScenario() {
  if (!form.value.name) return;
  const snap = { ...form.value };

  // Derived metrics
  const pp = Number(snap.purchasePrice) || 0;
  const eq = Number(snap.equityInjection) || 0;
  const sde = Number(snap.sdeMultiple) || 0;
  const rev = Number(snap.projectedRevenue) || 0;
  const margin = Number(snap.projectedMargin) || 0;

  snap._loanAmount = pp - eq;
  snap._impliedValue = sde > 0 ? pp / sde : null;
  snap._netIncome = rev * (margin / 100);

  // Simple annual debt service estimate (SBA only for now)
  const principal = snap._loanAmount;
  const r = (Number(snap.sbaRate) || 0) / 100 / 12;
  const n = (Number(snap.sbaTerm) || 10) * 12;
  const monthlyPayment = r > 0 ? principal * r / (1 - Math.pow(1 + r, -n)) : (principal / n);
  snap._annualDebtService = monthlyPayment * 12;
  snap._dscr = snap._annualDebtService > 0 ? snap._netIncome / snap._annualDebtService : null;

  if (editIndex.value !== null) {
    scenarios.value[editIndex.value] = snap;
  } else {
    if (scenarios.value.length >= 4) return;
    scenarios.value.push(snap);
  }

  formVisible.value = false;
}

function removeScenario(i) {
  scenarios.value.splice(i, 1);
}

function fmt(val, type = 'currency') {
  if (val == null || val === '' || isNaN(val)) return '—';
  if (type === 'currency') {
    return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD', maximumFractionDigits: 0 }).format(val);
  }
  if (type === 'pct') return Number(val).toFixed(2) + '%';
  if (type === 'x') return Number(val).toFixed(2) + 'x';
  return String(val);
}

function dscrColor(val) {
  if (val == null) return '#4a4e58';
  if (val < 1.0) return '#ef4444';
  if (val < 1.25) return '#f59e0b';
  return '#3ecf8e';
}

const compareRows = [
  { label: 'Purchase Price',      key: 'purchasePrice',      type: 'currency' },
  { label: 'Equity Injection',    key: 'equityInjection',    type: 'currency' },
  { label: 'Loan Amount (est.)',  key: '_loanAmount',         type: 'currency' },
  { label: 'SBA Rate',            key: 'sbaRate',             type: 'pct' },
  { label: 'SBA Term (yr)',       key: 'sbaTerm',             type: 'raw' },
  { label: 'Seller Note',         key: 'sellerNoteAmount',    type: 'currency' },
  { label: 'Projected Revenue',   key: 'projectedRevenue',    type: 'currency' },
  { label: 'Projected Margin',    key: 'projectedMargin',     type: 'pct' },
  { label: 'Net Income (est.)',   key: '_netIncome',           type: 'currency' },
  { label: 'Annual Debt Svc',     key: '_annualDebtService',  type: 'currency' },
  { label: 'DSCR (est.)',         key: '_dscr',                type: 'x' },
];
</script>

<template>
  <div class="sc-wrap">

    <div class="page-header">
      <div class="header-eyebrow">Side-by-Side Analysis</div>
      <h1 class="page-title">Scenario Comparison</h1>
      <p class="page-sub">Model up to 4 deal structures and compare them side-by-side.</p>
    </div>

    <!-- Action bar -->
    <div class="action-bar">
      <button
        class="btn-add"
        :disabled="scenarios.length >= 4"
        @click="openForm()"
      >
        + Add Scenario
      </button>
      <span class="count-label" v-if="scenarios.length">
        {{ scenarios.length }} / 4 scenarios
      </span>
    </div>

    <!-- Empty state -->
    <div v-if="scenarios.length === 0 && !formVisible" class="empty-state">
      <div class="empty-icon">⊞</div>
      <p class="empty-title">No scenarios yet</p>
      <p class="empty-sub">Add your first deal structure to begin comparing acquisition scenarios.</p>
      <button class="btn-add" @click="openForm()">+ Add Scenario</button>
    </div>

    <!-- Comparison table -->
    <div v-if="scenarios.length > 0" class="compare-table">
      <table>
        <thead>
          <tr>
            <th class="row-label-head">Metric</th>
            <th v-for="(s, i) in scenarios" :key="i" class="scenario-head">
              <div class="scenario-head-inner">
                <span class="scenario-name">{{ s.name }}</span>
                <div class="scenario-actions">
                  <button class="icon-btn" @click="openForm(i)" title="Edit">✎</button>
                  <button class="icon-btn danger" @click="removeScenario(i)" title="Remove">✕</button>
                </div>
              </div>
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="row in compareRows" :key="row.key" class="data-row">
            <td class="row-label">{{ row.label }}</td>
            <td
              v-for="(s, i) in scenarios"
              :key="i"
              class="data-cell"
              :style="row.key === '_dscr' ? { color: dscrColor(s[row.key]) } : {}"
            >
              {{ fmt(s[row.key], row.type) }}
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Form overlay -->
    <div class="form-overlay" v-if="formVisible" @click.self="formVisible = false">
      <div class="form-panel">
        <div class="form-header">
          <span class="form-title">{{ editIndex !== null ? 'Edit Scenario' : 'New Scenario' }}</span>
          <button class="icon-btn" @click="formVisible = false">✕</button>
        </div>

        <div class="form-body">
          <div class="form-section">
            <span class="form-section-label">Identity</span>
            <div class="field">
              <label>Scenario Name</label>
              <input v-model="form.name" type="text" placeholder="e.g. Conservative Base Case" />
            </div>
          </div>

          <div class="form-section">
            <span class="form-section-label">Acquisition Structure</span>
            <div class="field-row">
              <div class="field">
                <label>Purchase Price ($)</label>
                <input v-model="form.purchasePrice" type="number" placeholder="1,500,000" />
              </div>
              <div class="field">
                <label>SDE Multiple (x)</label>
                <input v-model="form.sdeMultiple" type="number" step="0.1" placeholder="3.5" />
              </div>
            </div>
            <div class="field">
              <label>Equity Injection ($)</label>
              <input v-model="form.equityInjection" type="number" placeholder="150,000" />
            </div>
          </div>

          <div class="form-section">
            <span class="form-section-label">SBA Loan</span>
            <div class="field-row">
              <div class="field">
                <label>Interest Rate (%)</label>
                <input v-model="form.sbaRate" type="number" step="0.1" placeholder="7.5" />
              </div>
              <div class="field">
                <label>Term (years)</label>
                <input v-model="form.sbaTerm" type="number" placeholder="10" />
              </div>
            </div>
          </div>

          <div class="form-section">
            <span class="form-section-label">Seller Note</span>
            <div class="field-row">
              <div class="field">
                <label>Amount ($)</label>
                <input v-model="form.sellerNoteAmount" type="number" placeholder="0" />
              </div>
              <div class="field">
                <label>Rate (%)</label>
                <input v-model="form.sellerNoteRate" type="number" step="0.1" placeholder="5.0" />
              </div>
            </div>
          </div>

          <div class="form-section">
            <span class="form-section-label">Projections</span>
            <div class="field-row">
              <div class="field">
                <label>Revenue ($)</label>
                <input v-model="form.projectedRevenue" type="number" placeholder="2,000,000" />
              </div>
              <div class="field">
                <label>Net Margin (%)</label>
                <input v-model="form.projectedMargin" type="number" step="0.1" placeholder="18.0" />
              </div>
            </div>
          </div>
        </div>

        <div class="form-footer">
          <button class="btn-cancel" @click="formVisible = false">Cancel</button>
          <button class="btn-save" @click="saveScenario">
            {{ editIndex !== null ? 'Update' : 'Save Scenario' }}
          </button>
        </div>
      </div>
    </div>

  </div>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=DM+Mono:wght@400;500&family=Fraunces:ital,opsz,wght@0,9..144,300;0,9..144,600;1,9..144,300&display=swap');

.sc-wrap {
  width: 100%;
  min-height: 100%;
  padding: 40px 36px 60px;
  box-sizing: border-box;
  color: #e8e4dc;
  position: relative;
}

/* Header */
.page-header { margin-bottom: 28px; }
.header-eyebrow {
  font-size: 10px;
  letter-spacing: 0.22em;
  color: #c9a96e;
  text-transform: uppercase;
  margin-bottom: 8px;
}
.page-title {
  font-size: 30px;
  font-weight: 300;
  color: #000000;
  margin-bottom: 6px;
}
.page-sub { font-size: 11px; color: #3a3e48; }

/* Action bar */
.action-bar {
  display: flex;
  align-items: center;
  gap: 16px;
  margin-bottom: 24px;
}

.btn-add {
  background: #c9a96e;
  color: #0b0d0f;
  border: none;
  padding: 9px 18px;
  border-radius: 6px;
  font-family: 'DM Mono', monospace;
  font-size: 11px;
  font-weight: 500;
  letter-spacing: 0.06em;
  cursor: pointer;
  transition: opacity 0.18s;
}
.btn-add:disabled { opacity: 0.3; cursor: not-allowed; }
.btn-add:not(:disabled):hover { opacity: 0.85; }

.count-label {
  font-size: 10px;
  color: #3a3e48;
  letter-spacing: 0.08em;
}

/* Empty state */
.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
  padding: 80px 20px;
  border: 1px dashed #1e2025;
  border-radius: 14px;
}
.empty-icon { font-size: 32px; color: #2a2d34; }
.empty-title { font-size: 14px; color: #6b7280; }
.empty-sub { font-size: 11px; color: #3a3e48; text-align: center; max-width: 340px; }

/* Comparison table */
.compare-table {
  overflow-x: auto;
  border: 1px solid #1e2025;
  border-radius: 12px;
  background: #111316;
}

table {
  width: 100%;
  border-collapse: collapse;
  font-size: 11px;
}

th, td {
  padding: 11px 16px;
  text-align: left;
  border-bottom: 1px solid #15171a;
}

tr:last-child td { border-bottom: none; }

.row-label-head {
  font-size: 10px;
  letter-spacing: 0.1em;
  color: #3a3e48;
  text-transform: uppercase;
  background: #0e1013;
  border-right: 1px solid #1e2025;
  width: 180px;
}

.scenario-head {
  background: #0e1013;
  border-right: 1px solid #1e2025;
}

.scenario-head:last-child { border-right: none; }

.scenario-head-inner {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 10px;
}

.scenario-name {
  font-size: 12px;
  color: #c9a96e;
  letter-spacing: 0.04em;
}

.scenario-actions {
  display: flex;
  gap: 4px;
}

.icon-btn {
  background: transparent;
  border: none;
  color: #4a4e58;
  cursor: pointer;
  font-size: 11px;
  padding: 3px 5px;
  border-radius: 4px;
  transition: color 0.15s, background 0.15s;
}
.icon-btn:hover { color: #e8e4dc; background: #1e2025; }
.icon-btn.danger:hover { color: #ef4444; background: rgba(239,68,68,0.1); }

.row-label {
  color: #5a5e6a;
  font-size: 11px;
  border-right: 1px solid #1e2025;
  white-space: nowrap;
}

.data-row:nth-child(even) { background: #0e1013; }

.data-cell {
  color: #c8c4bc;
  border-right: 1px solid #15171a;
}
.data-cell:last-child { border-right: none; }

/* Form overlay */
.form-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.7);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
}

.form-panel {
  background: #111316;
  border: 1px solid #1e2025;
  border-radius: 14px;
  width: min(560px, 95vw);
  max-height: 90vh;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
}

.form-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 18px 22px;
  border-bottom: 1px solid #1e2025;
  background: #0e1013;
}

.form-title {
  font-size: 13px;
  letter-spacing: 0.08em;
  color: #c9a96e;
}

.form-body {
  padding: 22px;
  display: flex;
  flex-direction: column;
  gap: 22px;
}

.form-section {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.form-section-label {
  font-size: 9px;
  letter-spacing: 0.18em;
  text-transform: uppercase;
  color: #4a4e58;
  border-bottom: 1px solid #1a1d22;
  padding-bottom: 6px;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 5px;
  flex: 1;
}

.field label {
  font-size: 10px;
  color: #5a5e6a;
  letter-spacing: 0.06em;
}

.field input {
  background: #0b0d0f;
  border: 1px solid #1e2025;
  border-radius: 6px;
  padding: 8px 10px;
  color: #e8e4dc;
  font-family: 'DM Mono', monospace;
  font-size: 12px;
  outline: none;
  transition: border-color 0.18s;
}
.field input:focus { border-color: #c9a96e; }
.field input::placeholder { color: #2a2d34; }

.field-row {
  display: flex;
  gap: 12px;
}

.form-footer {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  padding: 16px 22px;
  border-top: 1px solid #1e2025;
}

.btn-cancel {
  background: transparent;
  border: 1px solid #1e2025;
  color: #5a5e6a;
  padding: 8px 16px;
  border-radius: 6px;
  font-family: 'DM Mono', monospace;
  font-size: 11px;
  cursor: pointer;
  transition: border-color 0.18s, color 0.18s;
}
.btn-cancel:hover { border-color: #3a3e48; color: #e8e4dc; }

.btn-save {
  background: #c9a96e;
  border: none;
  color: #0b0d0f;
  padding: 8px 18px;
  border-radius: 6px;
  font-family: 'DM Mono', monospace;
  font-size: 11px;
  font-weight: 500;
  cursor: pointer;
  transition: opacity 0.18s;
}
.btn-save:hover { opacity: 0.85; }
</style>
