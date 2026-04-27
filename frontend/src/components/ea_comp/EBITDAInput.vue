<script setup>

const props = defineProps({
    label: {type: String, required: true},
    modelValue: {type: Number, required: true}
})

const emit = defineEmits(['update:modelValue']);

const onInput = (event) => {
    const value = event.target.value;
    const parsed = value === "" ? null : Number(value);
    emit("update:modelValue", parsed);
}

</script>

<template>

    <label>{{ label }}</label>
    <input type="number" :value = "modelValue" @input="onInput" />

</template>

<style scoped>
input {
    padding: 10px 12px;
    border-radius: 8px;
    border: 1px solid transparent;
    box-sizing: border-box;
    outline: none;
    font-size: 13px;
    background: #f9fafb;
    transition: all 0.2s ease;
    font-family: 'Arial', sans-serif;
}

input:hover {
    background: #f3f4f6;
}

input:focus {
    background: #ffffff;
    border-color: #111827;
    box-shadow: 0 0 0 2px rgba(0,0,0,0.05);
}
</style>