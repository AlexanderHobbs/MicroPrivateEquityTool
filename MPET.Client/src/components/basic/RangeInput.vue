<script setup>
const props = defineProps({
    label: { type: String, required: true },
    modelValue: { type: Number, default: null },  
    placeholder: { type: String, default: "00.00" },
    type: {type: String, default: "currency"},
    class: { type: String, default: "range" },
    icon: { type: String, default: "/src/assets/navbar-icons/break-even.png" }
});

const emit = defineEmits(['update:modelValue']);

const onInput = (event) => {

    const value = event.target.value;
        
    const parsed = value === "" ? 0 : Number(value);
       
    emit("update:modelValue", parsed);
    
}

</script>

<template>
    <div :class="props.class">

        <div class="label">
            <label>{{ props.label }}</label> 
            <div v-if = "type === 'percent'">
            <input class = "place-value"
                type="number" 
                :placeholder="props.modelValue + ' %'"
                :value = "props.modelValue"
                @input = "onInput"
                >%
            </div>
            <div  v-if = "type === 'currency'">
            <input class = "place-value"
                type="number" 
                :placeholder="props.modelValue + ' $'"
                :value = "props.modelValue"
                @input = "onInput"
                > $
            </div>
        </div>

        <input v-if = "type === 'percent'"
            type="range" 
            :value="props.modelValue" 
            @input="onInput" 
            min="4" 
            max="12" 
            step=".5"/>

        <input v-else-if = "type === 'currency'"
            type="range" 
            :value="props.modelValue" 
            @input="onInput" 
            min="-5000000" 
            max="5000000" 
            step="1"/>
        
    </div>
</template>

<style scoped>

.range {
    width: 100%;
    height: auto;
    display: flex;
    flex-direction: column;
    gap: 5px;
}

input {
    flex: 2;
    padding: 15px 10px;
    border-radius: 6px;
    border: 1px solid #e6e8ec;
    outline: none;
    font-size: 13px;
    background: #f9fafb;
    transition: all 0.15s ease;
    font-family: inherit;
}

input::placeholder {
    color: #9ca3af;
}

input:hover {
    background: #f3f4f6;
}

input:focus {
    background: #ffffff;
    border-color: #d1d5db;
    box-shadow: 0 0 0 1px rgba(17, 24, 39, 0.05);
}

.place-value {
    padding: 0;
    border: none;
    background: transparent;
    font-size: 16px;
    font-weight: 500;
    color: #6b7280;
    text-align: center;
}

.range .label {
    flex: 1;
}

.range input {
    flex: 2;
}


.label {
    flex: 1;
    display: flex;
    justify-content: space-between;
    text-align: left;
}

.label label {
    flex: 1;
}

label {
    font-size: 16px;
    font-weight: 500;
    color: #6b7280;
}

</style>