<script setup>

const props = defineProps({  
    label: { type: String, required: true },
    modelValue: { type: [Number, String], default: null },  
    placeholder: { type: String, default: "00.00" },
    inputType: { type: Number, default: 1 },
    class: { type: String, default: "single-input" },
    icon: { type: String, default: "/src/assets/navbar-icons/break-even.png" }  
});

const emit = defineEmits(['update:modelValue']);

const onInput = (event) => {
    const value = event.target.value;

    if (props.inputType === 1 || props.inputType === 4) {
        const parsed = value === "" ? null : Number(value);
        emit("update:modelValue", parsed);
    } else {
        emit("update:modelValue", value);
    }
}

</script>

<template>
   
    <div :class="props.class" v-if="props.inputType === 1">
        <div class="label"><label>{{ props.label }}</label></div>
        <input 
            type="number" 
            :placeholder="props.placeholder" 
            :value="props.modelValue" 
            @input="onInput"/>
    </div>

    <div :class="props.class" v-else-if="props.inputType === 2">
        <label>{{ props.label }}</label>
        <input 
            type="text" 
            :value="props.modelValue" 
            @input="onInput" 
            :placeholder="props.placeholder"/>
    </div>

    <div :class="props.class" v-else-if="props.inputType === 3">
        <label>{{ props.label }}</label>
        <textarea 
            :value="props.modelValue" 
            @input="onInput" 
            :placeholder="props.placeholder">
        </textarea>
    </div>

    <div :class="props.class + '-range'" v-else-if="props.inputType === 4">
        <div class="label">
            <label>{{ props.label }}</label> <label>{{ props.modelValue }}%</label>
        </div>
        <input 
            type="range" 
            :value="props.modelValue" 
            @input="onInput" 
            min="-1" 
            max="3" 
            step=".5"/>
    </div>

     <div :class="props.class" v-if="props.inputType === 5">
        <input class="label"
            type = "text"
            :value = "props.label"
            @input = "onInput"
        />
        <input 
            type="number" 
            :placeholder="props.placeholder" 
            :value="props.modelValue" 
            @input="onInput"/>
    </div>
    
</template>

<style scoped>
.single-input {
    display: flex;
    align-items: center;
    gap: 12px;
    background: #ffffff;
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

.single-input input {
    flex: 1;
}

img {
    height: 20px;
    width: auto;
    border-radius: 7px;
    background-color: #f1f2f2;
    padding: 8px;
}

.no-border {
    display: flex;
    align-items: center;
    gap: 60px;
}

label {
    font-size: 16px;
    font-weight: 500;
    color: #6b7280;
}

input,
textarea {
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

input:hover,
textarea:hover {
    background: #f3f4f6;
}

input:focus,
textarea:focus {
    background: #ffffff;
    border-color: #d1d5db;
    box-shadow: 0 0 0 1px rgba(17, 24, 39, 0.05);
}

.single-input-range {
    width: 100%;
    height: auto;
    display: flex;
    flex-direction: column;
    gap: 5px;
}

.range {
    width: 100%;
    height: auto;
    display: flex;
    flex-direction: column;
    gap: 5px;
}

.range .label {
    flex: 1;
}

.range input {
    flex: 2;
}
</style>