using Checker_v._3._0.Helpers;
using Checker_v._3._0.Models;
using Checker_v._3._0.Models.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Controllers
{
    public class EntityObjectsController : Controller
    {
        private DataContext dataContext;
        public EntityObjectsController(DataContext context)
        {
            dataContext = context;
        }

        [Route("/EntityObjects/{entityType}/List")]
        public ActionResult List(string entityType)
        {
            var type = Type.GetType($"Checker_v._3._0.Models.{entityType}");

            if (type == null)
                return ResultHelper.EntityNotFound(entityType);

            var entityDisplayName = (type.GetCustomAttributes(false).First(attr => attr is ListDisplay) as ListDisplay).Name;

            var fields = type.GetProperties()
                .Where(p => p.GetCustomAttributes(false).Any(attr => attr is ListDisplay))
                .Select(p => new 
                { 
                    FieldName = p.Name,
                    FieldType = p.PropertyType,
                    FieldDisplayName = (p.GetCustomAttributes(false).First(attr => attr is ListDisplay) as ListDisplay).Name
                });

            var head = fields.Select(x => x.FieldDisplayName)
                .ToList();

            var query = (IQueryable)type.GetMethod("AsIQueryable").Invoke(EntityObject.GetInstance(dataContext, type), new object[] { dataContext, type });

            var data = query.Cast<EntityObject>().ToList();
            var resultData = new List<List<EntityObjectFieldDto>>();

            foreach(var entity in data)
            {
                var dtoEntity = new List<EntityObjectFieldDto>();
                foreach(var field in fields)
                {
                    if (field.FieldType.Name == "Int32")
                    {
                        dtoEntity.Add(new EntityObjectFieldDto()
                        {
                            Name = field.FieldName,
                            Title = field.FieldDisplayName,
                            Type = field.FieldType,
                            Value = entity.GetType().GetProperty(field.FieldName).GetValue(entity, null),
                            Url = null
                        });
                    }
                    else if (field.FieldType.Name == "Nullable`1")//nullable int
                    {
                        var fieldValue = (int?)entity.GetType().GetProperty(field.FieldName).GetValue(entity, null);

                        dtoEntity.Add(new EntityObjectFieldDto()
                        {
                            Name = field.FieldName,
                            Title = field.FieldDisplayName,
                            Type = field.FieldType,
                            Value = fieldValue == null ? null : fieldValue,
                            Url = null
                        });
                    }
                    else if (field.FieldType.Name == "String")
                    {
                        var url = (string)null;
                        if (field.FieldName == "Title")
                            url = "https://" + HttpContext.Request.Host + entity.RouteDetail();

                        dtoEntity.Add(new EntityObjectFieldDto()
                        {
                            Name = field.FieldName,
                            Title = field.FieldDisplayName,
                            Type = field.FieldType,
                            Value = entity.GetType().GetProperty(field.FieldName).GetValue(entity, null),
                            Url = url
                        });
                    }else if (field.FieldType.IsGenericType && field.FieldType.Name == "IList`1")
                    {
                        dynamic items = (dynamic)entity.GetType().GetProperty(field.FieldName).GetValue(entity, null);
                        //var values = new List<EntityObjectFieldDto>();
                        //foreach(var item in items)
                        //{
                        //    var url = EntityObject.GetInstance(dataContext, item.GetType()).RouteList();
                        //    values.Add(new EntityObjectFieldDto() 
                        //    { 
                        //        Type = item.GetType(),
                        //        Value = item.GetType().GetProperty("Title").GetValue(item, null),
                        //        Url = url
                        //    });
                        //}
                        dtoEntity.Add(new EntityObjectFieldDto()
                        {
                            Name = field.FieldName,
                            Title = field.FieldDisplayName,
                            Type = typeof(List<EntityObject>),
                            Value = items.Count,
                        });
                    }
                    else if (field.FieldType.BaseType.Name == "EntityObject")
                    {
                        var fieldValue = entity.GetType().GetProperty(field.FieldName).GetValue(entity, null);
                        var url = fieldValue == null ? "" : (fieldValue as EntityObject).RouteDetail();
                        dtoEntity.Add(new EntityObjectFieldDto()
                        {
                            Name = field.FieldName,
                            Title = field.FieldDisplayName,
                            Type = field.FieldType,
                            Value = fieldValue == null ? "" : fieldValue.GetType().GetProperty("Title").GetValue(fieldValue, null),
                            Url = "https://" + HttpContext.Request.Host + url
                        });
                    }
                }
                resultData.Add(dtoEntity);
            }

            return View(new EntityObjectListDto() { 
                EntityName = entityType,
                Title = entityDisplayName,
                Head = head,
                Entities = resultData
            });
        }

        [Route("/EntityObjects/{entityType}/Detail/{id}")]
        public ActionResult Detail(string entityType, int id)
        {
            var type = Type.GetType($"Checker_v._3._0.Models.{entityType}");

            if (type == null)
                return ResultHelper.EntityNotFound(entityType);

            var fields = type.GetProperties()
                .Where(p => p.GetCustomAttributes(false).Any(attr => attr is DetailDisplay))
                .Select(p => new
                {
                    FieldName = p.Name,
                    FieldType = p.PropertyType,
                    FieldDisplayName = (p.GetCustomAttributes(false).First(attr => attr is DetailDisplay) as DetailDisplay).Name
                });

            var entity = (EntityObject)dataContext.Set(type).Find(id);

            if(entity == null)
                return ResultHelper.EntityNotFound($"{id}");

            var entityFields = new List<EntityObjectFieldDto>();
            foreach (var field in fields)
            {
                if (field.FieldType.Name == "Int32")
                {
                    entityFields.Add(new EntityObjectFieldDto()
                    {
                        Type = field.FieldType,
                        Title = field.FieldDisplayName,
                        Value = entity.GetType().GetProperty(field.FieldName).GetValue(entity, null),
                        Url = null
                    });
                }
                else if (field.FieldType.Name == "String")
                {
                    entityFields.Add(new EntityObjectFieldDto()
                    {
                        Type = field.FieldType,
                        Title = field.FieldDisplayName,
                        Value = entity.GetType().GetProperty(field.FieldName).GetValue(entity, null),
                        Url = null
                    });
                }
                else if (field.FieldType.IsGenericType && field.FieldType.Name == "IList`1")
                {
                    dynamic items = (dynamic)entity.GetType().GetProperty(field.FieldName).GetValue(entity, null);

                    entityFields.Add(new EntityObjectFieldDto()
                    {
                        Type = typeof(List<EntityObject>),
                        Title = field.FieldDisplayName,
                        Value = items.Count,
                    });
                }
                else if (field.FieldType.BaseType.Name == "EntityObject")
                {
                    var fieldValue = entity.GetType().GetProperty(field.FieldName).GetValue(entity, null);
                    var url = fieldValue == null ? "" : (fieldValue as EntityObject).RouteDetail();
                    entityFields.Add(new EntityObjectFieldDto()
                    {
                        Type = field.FieldType,
                        Title = field.FieldDisplayName,
                        Value = fieldValue == null ? "" : fieldValue.GetType().GetProperty("Title").GetValue(fieldValue, null),
                        Url = "https://" + HttpContext.Request.Host + url
                    });
                }
            }

            return View(new EntityObjectDetailDto()
            {
                EntityId = id,
                EntityName = entityType,
                EntityTitle = (string)entity.GetType().GetProperty("Title").GetValue(entity, null),
                Fields = entityFields
            });
        }

        [HttpGet]
        [Route("/EntityObjects/{entityType}/Create")]
        public ActionResult Create(string entityType)
        {
            var type = Type.GetType($"Checker_v._3._0.Models.{entityType}");

            if (type == null)
                return ResultHelper.EntityNotFound(entityType);

            var entityDisplayName = (type.GetCustomAttributes(false).First(attr => attr is EditDisplay) as EditDisplay).Name;

            var fields = type.GetProperties()
                .Where(p => p.GetCustomAttributes(false).Any(attr => attr is EditDisplay))
                .Select(p => new
                {
                    FieldName = p.PropertyType.BaseType.Name == "EntityObject" ?
                                (p.GetCustomAttributes(false).First(attr => attr is ForeignKeyAttribute) as ForeignKeyAttribute).Name :
                                p.Name,
                    FieldType = p.PropertyType,
                    FieldDisplayName = (p.GetCustomAttributes(false).First(attr => attr is EditDisplay) as EditDisplay).Name,
                    FieldInputType = (p.GetCustomAttributes(false).First(attr => attr is InputType) as InputType).Name
                });

            var dtoEntity = new List<EntityObjectFieldDto>();
            foreach (var field in fields)
            {
                if (field.FieldType.Name == "Int32")
                {
                    dtoEntity.Add(new EntityObjectFieldDto()
                    {
                        Name = field.FieldName,
                        Title = field.FieldDisplayName,
                        Type = field.FieldType,
                        InputType = field.FieldInputType
                    });
                }
                else if (field.FieldType.Name == "Nullable`1")//nullable int
                {
                    dtoEntity.Add(new EntityObjectFieldDto()
                    {
                        Name = field.FieldName,
                        Title = field.FieldDisplayName,
                        Type = field.FieldType,
                        InputType = field.FieldInputType
                    });
                }
                else if (field.FieldType.Name == "String")
                {
                    dtoEntity.Add(new EntityObjectFieldDto()
                    {
                        Name = field.FieldName,
                        Title = field.FieldDisplayName,
                        Type = field.FieldType,
                        InputType = field.FieldInputType
                    });
                }
                else if (field.FieldType.BaseType.Name == "EntityObject")
                {
                    var items = ((IQueryable<dynamic>)dataContext.Set(field.FieldType))
                        .ToList()
                        .Select(x => new SelectListItem()
                        {
                            Value = $"{x.Id}",
                            Text = x.Title
                        }).ToList();
                        

                    dtoEntity.Add(new EntityObjectFieldDto()
                    {
                        Type = field.FieldType,
                        Title = field.FieldDisplayName,
                        Name = field.FieldName,
                        InputType = field.FieldInputType,
                        Values = items
                    });
                }
            }

            ViewData["Title"] = "Создать";

            return View("_CreateForm",new EntityObjectEditDto()
            {
                EntityName = entityDisplayName,
                Fields = dtoEntity
            });
        }

        [HttpPost]
        [Route("/EntityObjects/{entityType}/Create")]
        public ActionResult Create(string entityType, string dummy)
        {
            var data = JArray.Parse(this.HttpContext.Request.Form.Keys.First());

            var type = Type.GetType($"Checker_v._3._0.Models.{entityType}");

            if (type == null)
                return ResultHelper.EntityNotFound(entityType);

            var entity = EntityObject.GetInstance(dataContext, type);

            var fieldTypes = type.GetProperties()
                .Where(p => p.GetCustomAttributes(false).Any(attr => attr is EditDisplay))
                .ToDictionary(p => p.PropertyType.BaseType.Name == "EntityObject" ? 
                                   (p.GetCustomAttributes(false).First(attr => attr is ForeignKeyAttribute) as ForeignKeyAttribute).Name :
                                   p.Name, p => p.PropertyType);

            foreach (JObject field in data)
            {
                var fieldName = (string)field.GetValue("FieldName");
                var fieldType = fieldTypes[fieldName];
                var jFieldValue = field.GetValue("FieldValue");

                if(fieldName.EndsWith("_id"))
                {
                    entity.GetType().GetProperty(fieldName).SetValue(entity, (int)jFieldValue);
                }
                else
                {
                    entity.GetType().GetProperty(fieldName).SetValue(entity, Convert.ChangeType(jFieldValue, fieldType));
                }

                
            }

            dataContext.Entry(entity).State = EntityState.Added;
            dataContext.SaveChanges();

            return ResultHelper.Successed();
        }

        [HttpGet]
        [Route("/EntityObjects/{entityType}/Edit/{id}")]
        public ActionResult Edit(string entityType, int id)
        {
            var type = Type.GetType($"Checker_v._3._0.Models.{entityType}");

            if (type == null)
                return ResultHelper.EntityNotFound(entityType);

            var entity = (EntityObject)dataContext.Set(type).Find(id);

            if (entity == null)
                return ResultHelper.EntityNotFound($"{id}");

            var entityDisplayName = (type.GetCustomAttributes(false).First(attr => attr is EditDisplay) as EditDisplay).Name;

            var fields = type.GetProperties()
                .Where(p => p.GetCustomAttributes(false).Any(attr => attr is EditDisplay))
                .Select(p => new
                {
                    FieldName = p.PropertyType.BaseType.Name == "EntityObject" ?
                                (p.GetCustomAttributes(false).First(attr => attr is ForeignKeyAttribute) as ForeignKeyAttribute).Name :
                                p.Name,
                    FieldType = p.PropertyType,
                    FieldDisplayName = (p.GetCustomAttributes(false).First(attr => attr is EditDisplay) as EditDisplay).Name,
                    FieldInputType = (p.GetCustomAttributes(false).First(attr => attr is InputType) as InputType).Name
                });

            var dtoEntity = new List<EntityObjectFieldDto>();
            foreach (var field in fields)
            {
                if (field.FieldType.Name == "Int32")
                {
                    dtoEntity.Add(new EntityObjectFieldDto()
                    {
                        Name = field.FieldName,
                        Title = field.FieldDisplayName,
                        Type = field.FieldType,
                        InputType = field.FieldInputType,
                        Value = entity.GetType().GetProperty(field.FieldName).GetValue(entity, null),
                    });
                }
                else if (field.FieldType.Name == "Nullable`1")//nullable int
                {
                    var fieldValue = (int?)entity.GetType().GetProperty(field.FieldName).GetValue(entity, null);

                    dtoEntity.Add(new EntityObjectFieldDto()
                    {
                        Name = field.FieldName,
                        Title = field.FieldDisplayName,
                        Type = field.FieldType,
                        InputType = field.FieldInputType,
                        Value = fieldValue == null ? null : fieldValue,
                    });
                }
                else if (field.FieldType.Name == "String")
                {
                    dtoEntity.Add(new EntityObjectFieldDto()
                    {
                        Name = field.FieldName,
                        Title = field.FieldDisplayName,
                        Type = field.FieldType,
                        InputType = field.FieldInputType,
                        Value = entity.GetType().GetProperty(field.FieldName).GetValue(entity, null),
                    });
                }
                else if (field.FieldType.BaseType.Name == "EntityObject")
                {
                    var items = ((IQueryable<dynamic>)dataContext.Set(field.FieldType))
                        .ToList()
                        .Select(x => new SelectListItem()
                        {
                            Value = $"{x.Id}",
                            Text = x.Title
                        }).ToList();

                    var value = entity.GetType().GetProperty(field.FieldName).GetValue(entity, null);

                    var selectedItem = items.FirstOrDefault(x => x.Value == $"{value}");
                    if(selectedItem != null)
                        selectedItem.Selected = true;

                    dtoEntity.Add(new EntityObjectFieldDto()
                    {
                        Type = field.FieldType,
                        Title = field.FieldDisplayName,
                        Name = field.FieldName,
                        InputType = field.FieldInputType,
                        Values = items
                    });
                }
            }

            ViewData["Title"] = "Редактировать";

            return View("_CreateForm", new EntityObjectEditDto()
            {
                EntityName = entityDisplayName,
                Fields = dtoEntity
            });
        }

        [HttpPost]
        [Route("/EntityObjects/{entityType}/Edit/{id}")]
        public ActionResult Edit(string entityType, int id, int dummy)
        {
            var data = JArray.Parse(this.HttpContext.Request.Form.Keys.First());

            var type = Type.GetType($"Checker_v._3._0.Models.{entityType}");

            if (type == null)
                return ResultHelper.EntityNotFound(entityType);

            var entity = (EntityObject)dataContext.Set(type).Find(id);

            if (entity == null)
                return ResultHelper.EntityNotFound($"{id}");

            var fieldTypes = type.GetProperties()
                .Where(p => p.GetCustomAttributes(false).Any(attr => attr is EditDisplay))
                .ToDictionary(p => p.PropertyType.BaseType.Name == "EntityObject" ?
                                   (p.GetCustomAttributes(false).First(attr => attr is ForeignKeyAttribute) as ForeignKeyAttribute).Name :
                                   p.Name, p => p.PropertyType);

            foreach (JObject field in data)
            {
                var fieldName = (string)field.GetValue("FieldName");
                var fieldType = fieldTypes[fieldName];
                var jFieldValue = field.GetValue("FieldValue");

                if (fieldName.EndsWith("_id"))
                {
                    entity.GetType().GetProperty(fieldName).SetValue(entity, (int)jFieldValue);
                }
                else
                {
                    entity.GetType().GetProperty(fieldName).SetValue(entity, Convert.ChangeType(jFieldValue, fieldType));
                }


            }

            dataContext.Entry(entity).State = EntityState.Modified;
            dataContext.SaveChanges();

            return ResultHelper.Successed();
        }

        [HttpPost]
        [Route("/EntityObjects/{entityType}/Delete/{id}")]
        public ActionResult Delete(string entityType, int id)
        {
            var type = Type.GetType($"Checker_v._3._0.Models.{entityType}");

            if (type == null)
                return ResultHelper.EntityNotFound(entityType);

            var entity = (EntityObject)dataContext.Set(type).Find(id);

            if (entity == null)
                return ResultHelper.EntityNotFound($"{id}");

            dataContext.Entry(entity).State = EntityState.Deleted;
            dataContext.SaveChanges();

            return ResultHelper.Successed();
        }
    }
}
