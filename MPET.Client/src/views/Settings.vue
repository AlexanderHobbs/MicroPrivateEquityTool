<script setup>
const emit = defineEmits(['close'])

function closeSettings() {
  emit('close')
}
</script>

<template>
  <div class="settings-container">
    
    <!-- Header -->
    <div class="settings-header">
      <h2>Settings</h2>
      <button class="close-btn" @click="closeSettings">✕</button>
    </div>

    <!-- Content -->
    <div class="settings-content">

      <div class="section">
        <h3>General</h3>
        <div class="field">
          <label>Currency</label>
          <select>
            <option>USD</option>
            <option>EUR</option>
          </select>
        </div>

        <div class="field">
          <label>Theme</label>
          <select>
            <option>Light</option>
            <option>Dark</option>
          </select>
        </div>
      </div>

      <div class="section">
        <h3>Inputs</h3>
        <div class="field">
          <label>Default Growth Rate (%)</label>
          <input type="number" placeholder="5.0" />
        </div>

        <div class="field">
          <label>Inflation Rate (%)</label>
          <input type="number" placeholder="2.5" />
        </div>
      </div>

      <div class="section">
        <h3>Data</h3>
        <button class="danger">Reset All Data</button>
      </div>

    </div>

  </div>
</template>

<style scoped>
.settings-container {
  width: 100%;
  height: 100%;
  padding: 24px;
  background: #f9fafb;
  display: flex;
  flex-direction: column;
}

/* Header */
.settings-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.settings-header h2 {
  font-size: 20px;
  font-weight: 600;
}

.close-btn {
  background: transparent;
  border: none;
  font-size: 18px;
  cursor: pointer;
  padding: 6px 10px;
  border-radius: 6px;
  transition: background 0.2s ease;
}

.close-btn:hover {
  background: #e5e7eb;
}

/* Content */
.settings-content {
  display: flex;
  flex-direction: column;
  gap: 24px;
}

/* Sections */
.section {
  background: white;
  padding: 16px;
  border-radius: 10px;
  box-shadow: 0 2px 6px rgba(0,0,0,0.05);
}

.section h3 {
  margin-bottom: 12px;
  font-size: 16px;
  font-weight: 600;
}

/* Fields */
.field {
  display: flex;
  flex-direction: column;
  margin-bottom: 12px;
}

.field label {
  font-size: 13px;
  margin-bottom: 4px;
  color: #4b5563;
}

.field input,
.field select {
  padding: 8px;
  border-radius: 6px;
  border: 1px solid #d1d5db;
  font-size: 14px;
}

.field input:focus,
.field select:focus {
  outline: none;
  border-color: #16a34a;
}

/* Danger button */
.danger {
  background: #dc2626;
  color: white;
  padding: 8px 12px;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 500;
}

.danger:hover {
  opacity: 0.9;
}
</style>