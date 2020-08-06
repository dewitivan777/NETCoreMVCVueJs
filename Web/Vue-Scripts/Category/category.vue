<template>
    <div id="category">
        <v-app id="inspire">
            <v-data-table :headers="headers"
                          :items="categories"
                          sort-by="name"
                          class="elevation-1">
                <template v-slot:top>
                    <v-toolbar flat color="white">
                        <v-toolbar-title>Categories</v-toolbar-title>
                        <v-divider class="mx-4"
                                   inset
                                   vertical></v-divider>
                        <v-spacer></v-spacer>
                        <v-dialog v-model="dialog" max-width="500px">
                            <template v-slot:activator="{ on, attrs }">
                                <v-btn color="primary"
                                       dark
                                       class="mb-2"
                                       v-bind="attrs"
                                       v-on="on">New Item</v-btn>
                            </template>
                            <v-card>
                                <v-card-title>
                                    <span class="headline">{{ formTitle }}</span>
                                </v-card-title>

                                <v-card-text>
                                    <v-container>
                                        <v-row>
                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field v-model="editedItem.name" label="Name" :error-messages="modelstate['Name']"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field v-model="editedItem.description" label="Description" :error-messages="modelstate['Description']"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="12" md="12">
                                                <v-text-field v-model="editedItem.imageUrl" label="Image Url" :error-messages="modelstate['ImageUrl']"></v-text-field>
                                            </v-col>

                                        </v-row>
                                    </v-container>
                                </v-card-text>

                                <v-card-actions>
                                    <v-spacer></v-spacer>
                                    <v-btn color="blue darken-1" text @click="close">Cancel</v-btn>
                                    <v-btn color="blue darken-1" text @click="save">Save</v-btn>
                                </v-card-actions>
                            </v-card>
                        </v-dialog>

                    </v-toolbar>
                </template>
                <template v-slot:item.imageUrl="{ item }">
                    <div v-if="item.imageUrl">
                        <v-avatar>
                            <img :src="item.imageUrl" :alt="item.name">
                        </v-avatar>
                    </div>
                    <div v-else>
                        <v-avatar color="indigo">
                            <v-icon dark>mdi-signature-image</v-icon>
                        </v-avatar>
                    </div>
                </template>
                <template v-slot:item.actions="{ item }">
                    <v-icon small
                            class="mr-2"
                            @click="editItem(item)">
                        mdi-pencil
                    </v-icon>
                    <v-icon small
                            @click="deleteItem(item)">
                        mdi-delete
                    </v-icon>
                </template>
                <template v-slot:no-data>
                    <v-btn color="primary" @click="search">Reset</v-btn>
                </template>
            </v-data-table>
        </v-app>
    </div>

</template>


<script>
    export default {
        name: "category-component",
        data() {
            return {
                categories: [],
                modelstate: {},
                dialog: false,
                headers: [
                    { text: 'Avatar', value: 'imageUrl', sortable: false },
                    {
                        text: 'Id',
                        align: 'start',
                        sortable: false,
                        value: 'id',
                    },
                    { text: 'Name', value: 'name' },
                    { text: 'Description', value: 'description' },
                    { text: 'Actions', value: 'actions', sortable: false },
                ],
                editedIndex: -1,
                deleteIndex: -1,
                editedItem: {
                    id: '',
                    name: '',
                    description: '',
                    imageUrl: '',
                },
                defaultItem: {
                    id: '',
                    name: '',
                    description: '',
                    imageUrl: '',
                },
            }
        },
        computed: {
            formTitle() {
                return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
            },
        },
        watch: {
            dialog(val) {
                val
            },
        },
        methods: {
            search: function () {
                let self = this;
                this.$axios.get('/category/search')
                    .then(function (response) {
                        if (response.data.content != null) {
                            self.categories = response.data.content
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            editItem(item) {
                this.editedIndex = this.categories.indexOf(item)
                this.editedItem = Object.assign({}, item)
                this.dialog = true
            },
            deleteItem(item) {
                this.deleteIndex = this.categories.indexOf(item)
                this.editedItem = Object.assign({}, item)
                let self = this;

                confirm('Are you sure you want to delete this item?') &&
                    self.$axios({
                        method: 'post',
                        url: '/category/delete/'+ self.editedItem.id,
                    }).then((response) => {
                        console.log(response);
                        if (response.data.success) {
                            self.categories.splice(self.deleteIndex, 1)
                        }
                    }, (error) => {
                            console.log(error);
                    });
            },
            close() {
                this.dialog = false
                this.$nextTick(() => {
                    this.editedItem = Object.assign({}, this.defaultItem)
                    this.editedIndex = -1
                })
            },
            save() {
                let self = this;
                self.modelstate = {}
                if (this.editedIndex > -1) {
                    this.$axios({
                        method: 'post',
                        url: '/category/edit',
                        headers: { 'Content-Type': 'application/json' },
                        data: JSON.stringify(self.editedItem)
                    }).then((response) => {
                        if (!response.data.result.isError) {
                            Object.assign(self.categories[self.editedIndex], response.data.result.content)
                            self.close();
                        }
                    }, (error) => {
                        if (error.response.status == 400) {
                            self.modelstate = error.response.data;
                        }
                    });
                } else {
                    this.$axios({
                        method: 'post',
                        url: '/category/Add',
                        headers: { 'Content-Type': 'application/json' },
                        data: JSON.stringify(self.editedItem)
                    }).then((response) => {
                        if (!response.data.result.isError) {
                            self.categories.push(response.data.result.content)
                            self.close();
                        }
                    }, (error) => {
                        if (error.response.status == 400) {
                            self.modelstate = error.response.data;
                        }
                    });
                }
            },
        },
        mounted() {
            this.search()
        }
    };
</script>
